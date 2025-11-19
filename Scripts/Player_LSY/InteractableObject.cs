using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string InteractionText;
    public virtual void Interact()
    {
        Debug.Log("상호작용");
    }
}
