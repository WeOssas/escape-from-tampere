using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using escapefromtampere.Manager;

public class Interactable : MonoBehaviour
{
    public InputManager inputManager;
    public float radius = 3f;
    public float interactionCooldown = 1.5f;

    private Transform interactableObject;
    [SerializeField] private Transform player;
    private bool hasInteracted = false;

    private void Start()
    {
        interactableObject = GetComponent<Transform>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, interactableObject.position);
        if(distance <= radius & inputManager.Interact & !hasInteracted)
        {
            Interact();
        }
    }
    private void OnDrawGizmosSelected()
    {
        if(interactableObject == null)
            interactableObject = transform;
        
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactableObject.position, radius);
        
    }

    IEnumerator InteractCoolDown()
    {
        yield return new WaitForSeconds(interactionCooldown);
        hasInteracted = false;
        StopAllCoroutines();
    }

    public virtual void Interact()
    {
        Debug.Log("U INTERACTED WOW!");
        hasInteracted = true;
        StartCoroutine(InteractCoolDown());
    }
}
