using UnityEngine;

public abstract class Focuser : MonoBehaviour
{
    protected virtual void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1.5f))
        {
            IInteractable interactable = hit.transform.GetComponent<IInteractable>();
            if (interactable != null) interactable.Interact(this);
        }
    }
}
