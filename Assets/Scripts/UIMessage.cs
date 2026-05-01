using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UIMessage : MonoBehaviour
{
    public TMP_Text promptText;   // "Press E to ..."
    public TMP_Text messageText;  // "Item acquired"

    public float displayTime = 2f;

    private Coroutine currentRoutine;

    // 🔔 TEMP MESSAGE (top/center)
    public void ShowMessage(string message)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(Show(message));
    }

    IEnumerator Show(string message)
    {
        messageText.text = message;
        messageText.enabled = true;

        yield return new WaitForSeconds(displayTime);

        messageText.enabled = false;
    }

    // 🔽 Helper functions

    public void ShowNeedItem(string itemName)
    {
        ShowMessage($"You need a {itemName}");
    }

    public void ShowItemAcquired(string itemName)
    {
        ShowMessage($"{itemName} acquired");
    }

    // 🎯 PROMPT (always visible while looking)
    public void ShowInteractPrompt(string action)
    {
        promptText.text = $"Press E to {action}";
        promptText.enabled = true;
    }

    public void HidePrompt()
    {
        promptText.text = "";
        promptText.enabled = false;
    }
}