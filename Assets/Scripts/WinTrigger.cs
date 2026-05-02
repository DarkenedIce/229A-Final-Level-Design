using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager ui = FindFirstObjectByType<UIManager>();

            if (ui != null)
            {
                ui.ShowWin();
            }
        }
    }
}