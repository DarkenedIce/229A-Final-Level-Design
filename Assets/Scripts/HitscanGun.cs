using UnityEngine;

public class HitscanGun : MonoBehaviour
{
    public float range = 100f;
    public float fireRate = 10f;
    public float damage = 25f;

    public Camera playerCamera;
    public AudioSource audioSource;
    public AudioClip shootSFX;

    public GameObject gunObject;

    private float nextTimeToFire = 0f;
    private bool isGunActive = false;

    void Start()
    {
        if (gunObject != null)
            gunObject.SetActive(false);
    }

    void Update()
    {
        // Toggle gun
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    ToggleGun();
        //}

        if (isGunActive && Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    public void ActivateGun()
    {
        isGunActive = true;

        if (gunObject != null)
            gunObject.SetActive(true);

        Debug.Log("Gun acquired!");
    }

    //void ToggleGun()
    //{
    //    isGunActive = !isGunActive;

    //    if (gunObject != null)
    //        gunObject.SetActive(isGunActive);
    //}

    void Shoot()
    {
        if (shootSFX && audioSource)
            audioSource.PlayOneShot(shootSFX);

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            // Damage enemy
            EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            BossEnemy boss = hit.collider.GetComponent<BossEnemy>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }

            Debug.DrawLine(ray.origin, hit.point, Color.red, 1f);
        }
    }
}