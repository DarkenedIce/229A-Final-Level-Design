using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public string itemID = "Wrench";     // used internally
    public string itemName = "Wrench";   // shown to player

    public string GetInteractText()
    {
        return $"pick up {itemName}";
    }

    public void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            PlayerInteractor interactor = player.GetComponent<PlayerInteractor>();

            if (inventory != null)
            {
                inventory.AddKey(itemID); // still using same system
            }

            // UI message
            if (interactor != null && interactor.uiMessage != null)
            {
                interactor.uiMessage.ShowItemAcquired(itemName);
            }
        }

        Destroy(gameObject);
    }
}