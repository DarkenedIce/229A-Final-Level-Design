using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackRate = 1f;
    public int damage = 10;

    public float maxHP = 100f;

    private float currentHP;
    private float nextAttackTime = 0f;

    private Transform player;
    private NavMeshAgent agent;
    private bool isAlerted = false;

    private float alertTimer = 0f;
    public float alertDuration = 5f;

    [Header("Vision")]
    public Transform eyePoint;
    public LayerMask visionMask;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        currentHP = maxHP;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // 👁️ Detection phase (ONLY if not already alerted)
        if (!isAlerted && distance <= detectionRange)
        {
            Debug.Log("Player in range");

            if (HasLineOfSight())
            {
                Debug.Log("Line of sight TRUE");
                isAlerted = true;
            }
            else
            {
                Debug.Log("Line of sight FALSE");
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
            transform.rotation = Quaternion.LookRotation(dir);
    }

    void Attack()
    {
        agent.SetDestination(transform.position);

        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + 1f / attackRate;

            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(damage);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;

        // 🔥 IMPORTANT: alert enemy when shot
        isAlerted = true;
        alertTimer = alertDuration;

        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}