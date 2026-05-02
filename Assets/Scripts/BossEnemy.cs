using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 300f;
    private float currentHP;

    public int damage = 15;

    [Header("Movement")]
    public float moveSpeed = 4.5f;

    [Header("Detection")]
    public float detectionRange = 15f;
    public float attackRange = 2.5f;
    public float attackRate = 1f;

    [Header("Vision")]
    public Transform eyePoint;
    public LayerMask visionMask;

    private float nextAttackTime = 0f;
    private bool isAlerted = false;

    private Transform player;
    private NavMeshAgent agent;

    [Header("Drop")]
    public GameObject dropItemPrefab;

    void Start()
    {
        currentHP = maxHP;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        agent.speed = moveSpeed;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // 👁️ Detection phase
        if (!isAlerted && distance <= detectionRange)
        {
            if (HasLineOfSight())
            {
                isAlerted = true;
                Debug.Log("Boss spotted player!");
            }
        }

        // 🔥 Alert behavior
        if (isAlerted)
        {
            LookAtPlayer();

            if (distance > attackRange)
                agent.SetDestination(player.position);
            else
                Attack();
        }
    }

    bool HasLineOfSight()
    {
        Vector3 dir = (player.position - eyePoint.position).normalized;
        float distance = Vector3.Distance(eyePoint.position, player.position);

        RaycastHit hit;
        Debug.DrawRay(eyePoint.position, dir * distance, Color.red);

        if (Physics.Raycast(eyePoint.position, dir, out hit, distance, visionMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true; // nothing blocking
            }
        }

        return false;
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

        // 🔥 Become alerted when shot
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