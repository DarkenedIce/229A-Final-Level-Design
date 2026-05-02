using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHP = 100f;
    public TMP_Text hpText;
    private float currentHP;

    void Start()
    {
        currentHP = maxHP;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (hpText != null)
        {
            hpText.text = "HP: " + currentHP;
        }

        if (currentHP <= 30)
        {
            hpText.color = Color.red;
        }
        else
        {
            hpText.color = Color.green;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (currentHP < 0)
            currentHP = 0;

        UpdateUI();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died");

        GetComponent<FirstPersonController>().enabled = false;

        hpText.gameObject.SetActive(false);

        UIManager ui = FindFirstObjectByType<UIManager>();

        if (ui != null)
        {
            ui.ShowDeath();
        }
    }
}