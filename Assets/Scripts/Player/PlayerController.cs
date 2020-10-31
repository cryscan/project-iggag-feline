using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Stand,
    Move,
    Sprint,
}

public enum PlayerIdentity
{
    Guest,
    Worker,
    Guard,
}

public class PlayerStateChangeEvent
{
    public PlayerState current, previous;

    public PlayerStateChangeEvent(PlayerState current, PlayerState previous)
    {
        this.current = current;
        this.previous = previous;
    }
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerState _state;
    public PlayerState state { get => _state; }

    [SerializeField] PlayerIdentity _identity;
    public PlayerIdentity identity { get => _identity; }

    
}
