using UnityEngine;

public class PowerBox : MonoBehaviour, IInteractable
{
    public string requiredTool = "Wrench";
    public bool isFixed = false;
    public SlidingDoor slidingDoor;

    public string GetInteractText()
    {
        return isFixed ? "already fixed" : "fix";
    }

    public void Interact()
    {
        if (isFixed)
        {
            Debug.Log("Already fixed");
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerInventory inv = player.GetComponent<PlayerInventory>();
            PlayerInteractor interactor = player.GetComponent<PlayerInteractor>();

            if (inv != null)
            {
                if (!inv.HasKey(requiredTool))
                {
                    // Show UI message
                    if (interactor != null && interactor.uiMessage != null)
                    {
                        interactor.uiMessage.ShowMessage("You need a wrench");
                    }

                    return;
                }
            }
        }

        FixPowerBox();
    }

    void FixPowerBox()
    {
        isFixed = true;

        Debug.Log("Power box fixed!");

        // Open the door
        if (slidingDoor != null)
        {
            slidingDoor.Open();
        }
    }
}