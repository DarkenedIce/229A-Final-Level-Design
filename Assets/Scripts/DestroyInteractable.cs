using UnityEngine;

public class DestroyInteractable : MonoBehaviour, IInteractable
{
    public string objectName = "Object"; // for UI text

    public string GetInteractText()
    {
        return $"destroy {objectName}";
    }

    public void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerInteractor interactor = player.GetComponent<PlayerInteractor>();

            // Optional UI message
            if (interactor != null && interactor.uiMessage != null)
            {
                interactor.uiMessage.ShowMessage($"{objectName} destroyed");
            }
        }

        Destroy(gameObject);
    }
}