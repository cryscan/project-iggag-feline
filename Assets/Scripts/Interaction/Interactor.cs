using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    Collect,
    Drop,
    Hide,
    Reveal,
}

public interface Interactor
{
    InteractionType[] GetInteractions(Interactable interactable);
}
