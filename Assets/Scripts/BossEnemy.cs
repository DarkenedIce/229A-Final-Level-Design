using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 300f;
    private float currentHP;

    public int damage = 15;

    [Header("AI")]
    public float detectionRange = 15f;
    public float attackRange = 2.5f;
    public float attackRate = 1f;

    private float nextAttackTime = 0f;
    private bool isAlerted = false;

    private Transform player;
    private NavMeshAgent agent;

    [Header("Drop")]
    public GameObject dropItemPrefab; // key prefab

    void Start()
    {
        currentHP = maxHP;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (isAlerted || distance <= detectionRange)
        {
            LookAtPlayer();

            if (distance > attackRange)
            {
                agent.SetDestination(player.position);
            }
            else
            {
                Attack();
            }
        }
    }

    void LookAtPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    void Attack()
    {
        agent.SetDestination(transform.position);

        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + 1f / attackRate;

            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;

        // 🔥 Become aggressive when shot
        isAlerted = true;

        if (currentHP <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        DropItem();
        Destroy(gameObject);
    }

    void DropItem()
    {
        if (dropItemPrefab != null)
        {
            Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        }
    }
}