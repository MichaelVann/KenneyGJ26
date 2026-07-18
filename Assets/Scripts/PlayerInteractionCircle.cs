using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionCircle : MonoBehaviour
{
    [SerializeField] InputActionReference _interactAction;
    List<Interactable> m_inRangeInteractables;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_inRangeInteractables = new List<Interactable>();
    }

    void SortInRangeInteractablesByClosest()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_interactAction.action.ReadValue<float>() > 0)
        {
            SortInRangeInteractablesByClosest();
            if (m_inRangeInteractables.Count > 0)
            {
                m_inRangeInteractables[0].Interact();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D a_collision)
    {
        Interactable interactable = a_collision.gameObject.GetComponentInParent<Interactable>();
        if (interactable != null)
        {
            interactable.SetPromptActive(true);
            m_inRangeInteractables.Add(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D a_collision)
    {
        Interactable interactable = a_collision.gameObject.GetComponentInParent<Interactable>();
        if (interactable != null)
        {
            interactable.SetPromptActive(false);
            m_inRangeInteractables.Remove(interactable);
        }
    }
}
