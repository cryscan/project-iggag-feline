using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    public static GameContext instance { get; private set; }

    public Inventory[] inventories { get; private set; }
    public Collectable[] collectables { get; private set; }
    public DropPoint[] dropPoints { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        inventories = FindObjectsOfType<Inventory>();
        collectables = FindObjectsOfType<Collectable>();
        dropPoints = FindObjectsOfType<DropPoint>();
    }
}
