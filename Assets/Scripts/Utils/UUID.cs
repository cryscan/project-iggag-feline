/* * * * *
 * Unity Unique ID
 * ----------------
 * 
 * This component tries to solve the problem of generating a GUID for an object
 * that persists during development and at runtime and prevents any duplicates
 * of the ID. That means if an object is copied, cloned or instantiated the new
 * object should get a new ID. This also prevents accidental changes of an assigned
 * ID through "revert to prefab" or "apply". This is done by two static dictionaries
 * which track both, the ID as well as the components. Even after an assembly reload
 * all currently loaded objects / prefabs get immediately registrated again.
 * 
 * I've done several test to check if the ID correctly persists and it seems to work
 * in all cases. Though if you found a reproducible bug feel free to file an issue
 * https://github.com/Bunny83/UUID/issues
 * 
 * The MIT License (MIT)
 * 
 * Copyright (c) 2012-2017 Markus Göbel (Bunny83)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 * * * * */
using System.Collections.Generic;
using UnityEngine;

public class UUID : MonoBehaviour, ISerializationCallbackReceiver
{
    static Dictionary<UUID, string> obj2Uuid = new Dictionary<UUID, string>();
    static Dictionary<string, UUID> uuid2Obj = new Dictionary<string, UUID>();

    static void RegisterUUID(UUID id)
    {
        string uid;
        if (obj2Uuid.TryGetValue(id, out uid))
        {
            // found object instance, update ID
            id.uuid = uid;
            id.idBackup = id.uuid;
            if (!uuid2Obj.ContainsKey(uid))
                uuid2Obj.Add(uid, id);
            return;
        }

        if (string.IsNullOrEmpty(id.uuid))
        {
            // No ID yet, generate a new one.
            id.uuid = System.Guid.NewGuid().ToString();
            id.idBackup = id.uuid;
            uuid2Obj.Add(id.uuid, id);
            obj2Uuid.Add(id, id.uuid);
            return;
        }

        UUID tmp;
        if (!uuid2Obj.TryGetValue(id.uuid, out tmp))
        {
            // ID not known to the DB, so just register it
            uuid2Obj.Add(id.uuid, id);
            obj2Uuid.Add(id, id.uuid);
            return;
        }
        if (tmp == id)
        {
            // DB inconsistency
            obj2Uuid.Add(id, id.uuid);
            return;
        }
        if (tmp == null)
        {
            // object in DB got destroyed, replace with new
            uuid2Obj[id.uuid] = id;
            obj2Uuid.Add(id, id.uuid);
            return;
        }
        // we got a duplicate, generate new ID
        id.uuid = System.Guid.NewGuid().ToString();
        id.idBackup = id.uuid;
        uuid2Obj.Add(id.uuid, id);
        obj2Uuid.Add(id, id.uuid);
    }
    static void UnregisterUUID(UUID id)
    {
        uuid2Obj.Remove(id.uuid);
        obj2Uuid.Remove(id);
    }

    [SerializeField]
    private string uuid = null;
    private string idBackup = null;

    public string id { get { return uuid; } }

    public void OnAfterDeserialize()
    {
        if (uuid == null || uuid != idBackup)
            RegisterUUID(this);
    }
    public void OnBeforeSerialize()
    {
        if (uuid == null || uuid != idBackup)
            RegisterUUID(this);
    }
    void OnDestroy()
    {
        UnregisterUUID(this);
        uuid = null;
    }
}
