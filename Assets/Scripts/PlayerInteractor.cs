using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractor : MonoBehaviour
{
    public float range = 3f;
    public KeyCode interactKey = KeyCode.E;

    public Camera cam;
    public TMP_Text interactText;
    public UIMessage uiMessage;

    private IInteractable currentTarget;

    void Update()
    {
        Check();

        if (currentTarget != null && Input.GetKeyDown(interactKey))
        {
            currentTarget.Interact();
        }
    }

    void Check()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            var interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                currentTarget = interactable;

                if (uiMessage != null)
                {
                    // Default fallback
                    uiMessage.ShowInteractPrompt(interactable.GetInteractText());
                }

                return;
            }
        }

        currentTarget = null;

        if (uiMessage != null)
            uiMessage.HidePrompt();
    }
}