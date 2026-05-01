using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHP = 100f;
    private float currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        Debug.Log("Player HP: " + currentHP);

        if (currentHP <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died");

        GetComponent<FirstPersonController>().enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}