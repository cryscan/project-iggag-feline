using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    Collect,
    Drop,
    Hide,
    ComeOut,
}

public interface Interactor
{
    InteractionType[] GetInteractions(Interactable interactable);

    void Interact(Interactable interactable, InteractionType type);
}
