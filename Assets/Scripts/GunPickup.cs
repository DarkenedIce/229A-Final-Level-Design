using UnityEngine;

public class GunPickup : MonoBehaviour, IInteractable
{
    public string itemName = "Gun";

    public string GetInteractText()
    {
        return $"pick up {itemName}";
    }

    public void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            HitscanGun gun = player.GetComponent<HitscanGun>();
            PlayerInteractor interactor = player.GetComponent<PlayerInteractor>();

            if (gun != null)
            {
                gun.ActivateGun();
            }

            // ✅ SHOW UI MESSAGE
            if (interactor != null && interactor.uiMessage != null)
            {
                interactor.uiMessage.ShowItemAcquired(itemName);
            }
        }

        Destroy(gameObject);
    }
}