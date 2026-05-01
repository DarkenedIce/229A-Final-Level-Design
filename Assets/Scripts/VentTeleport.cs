using UnityEngine;

public class VentTeleport : MonoBehaviour, IInteractable
{
    public Transform exitPoint;

    public string GetInteractText()
    {
        return "enter vent";
    }

    public void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && exitPoint != null)
        {
            CharacterController controller = player.GetComponent<CharacterController>();

            if (controller != null)
            {
                controller.enabled = false;

                player.transform.position = exitPoint.position;
                player.transform.rotation = exitPoint.rotation;

                controller.enabled = true;
            }
            else
            {
                player.transform.position = exitPoint.position;
                player.transform.rotation = exitPoint.rotation;
            }
        }
    }
}