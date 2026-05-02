using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HitscanGun : MonoBehaviour
{
    [Header("Gun Stats")]
    public float range = 100f;
    public float fireRate = 10f;
    public float damage = 25f;

    [Header("Ammo")]
    public int magazineSize = 30;
    public int currentAmmo;
    public int reserveAmmo = 120;
    public float reloadTime = 2f;

    private bool isReloading = false;

    [Header("References")]
    public Camera playerCamera;
    public AudioSource audioSource;
    public AudioClip shootSFX;
    public AudioClip reloadSFX;

    public GameObject gunObject;

    [Header("UI")]
    public TMP_Text ammoText;

    private float nextTimeToFire = 0f;
    private bool isGunActive = false;

    void Start()
    {
        currentAmmo = magazineSize;

        if (gunObject != null)
            gunObject.SetActive(false);

        if (ammoText != null)
            ammoText.gameObject.SetActive(false); // 👈 ensure hidden at start
    }

    void Update()
    {
        if (!isGunActive) return;

        if (isReloading) return;

        if (currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        // Shoot
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        // Manual reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
        }
    }

    void Shoot()
    {
        currentAmmo--;

        if (shootSFX && audioSource)
            audioSource.PlayOneShot(shootSFX);

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
            if (enemy != null)
                enemy.TakeDamage(damage);

            BossEnemy boss = hit.collider.GetComponent<BossEnemy>();
            if (boss != null)
                boss.TakeDamage(damage);
        }

        UpdateUI();
    }

    void StartReload()
    {
        if (reserveAmmo <= 0 || currentAmmo == magazineSize)
            return;

        ammoText.text = "Reloading...";
        ammoText.color = Color.white;
        StartCoroutine(Reload());
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;

        if (reloadSFX && audioSource)
            audioSource.PlayOneShot(reloadSFX);

        yield return new WaitForSeconds(reloadTime);

        int neededAmmo = magazineSize - currentAmmo;
        int ammoToLoad = Mathf.Min(neededAmmo, reserveAmmo);

        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;

        isReloading = false;

        UpdateUI();
    }

    public void ActivateGun()
    {
        isGunActive = true;

        if (gunObject != null)
            gunObject.SetActive(true);

        UpdateUI();
    }

    void UpdateUI()
    {
        if (ammoText != null)
        {
            ammoText.gameObject.SetActive(isGunActive);

            if (isGunActive)
            {
                ammoText.text = currentAmmo + " / " + reserveAmmo;
                if (currentAmmo <= 2)
                    ammoText.color = Color.red;
                else
                    ammoText.color = Color.white;
            }
        }
    }
}