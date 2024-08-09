using UnityEngine;
using UnityEngine.UI; // Slider UI kullanýmý için gerekli
using Fusion;

public class PlayerFire : NetworkBehaviour
{
    public CharacterData _characterData;
    public GameObject bulletPrefab;
    public Transform firePoint;
    float nextFireTime;

    public int maxAmmo = 10; // Maksimum mermi sayýsý
    public int currentAmmo;
    public float reloadTime = 2f; // Yeniden yükleme süresi
    private bool isReloading = false;

    public Slider reloadBarUI; // Slider UI elemaný
    private float reloadProgress = 0f;

    private void Start()
    {
        currentAmmo = maxAmmo;

        if (firePoint == null)
        {
            Debug.LogError("FirePoint not found");
        }

        _characterData = GetComponent<Player>().Character;
        bulletPrefab = _characterData.projectilePrefab;

        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab not assigned!");
        }

        reloadBarUI.gameObject.SetActive(false); // Reload barýný baþlangýçta gizle
    }

    void Update()
    {
        if (isReloading)
        {
            HandleReload();
            return;
        }

        if (!HasInputAuthority)
            return;

        RotateFirePointToMouse();

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && currentAmmo > 0)
        {
            nextFireTime = Time.time + _characterData.FireRate;
            Shot();
        }

        if (currentAmmo <= 0 && !isReloading)
        {
            StartReloading();
        }
    }

    void RotateFirePointToMouse()
    {
        if (firePoint == null)
        {
            Debug.LogError("FirePoint not assigned!");
            return;
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 direction = (mousePosition - transform.position).normalized;

        firePoint.position = transform.position + (Vector3)direction;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Shot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab not assigned!");
            return;
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 direction = (mousePosition - firePoint.position).normalized;

        NetworkObject bullet = Runner.Spawn(bulletPrefab, firePoint.position, firePoint.rotation, Object.InputAuthority);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * _characterData.FireSpeed;
        }

        Mermi mer = bullet.GetComponent<Mermi>();
        if (mer != null)
        {
            mer.SetKnockBack(_characterData.KnockBackRate);
        }

        currentAmmo--;
        Debug.Log("Shot fired by client with InputAuthority: " + Object.InputAuthority + " | Current Ammo: " + currentAmmo);
    }

    void StartReloading()
    {
        isReloading = true;
        reloadProgress = 0f;
        reloadBarUI.gameObject.SetActive(true); // Yeniden yükleme barýný göster
        Debug.Log("Reloading...");
    }

    void HandleReload()
    {
        reloadProgress += Time.deltaTime / reloadTime;

        // UI barýný güncelleme
        if (reloadBarUI != null)
        {
            reloadBarUI.value = reloadProgress; // Barý doldurma oranýný güncelle
        }

        Debug.Log("Reload progress: " + reloadProgress);

        if (reloadProgress >= 1f)
        {
            FinishReloading();
        }
    }

    void FinishReloading()
    {
        Debug.Log("FinishReloading called");

        isReloading = false;
        currentAmmo = maxAmmo;
        reloadProgress = 0f; // Reload progress'i sýfýrla
        reloadBarUI.gameObject.SetActive(false); // Yeniden yükleme barýný gizle
        Debug.Log("Reloaded! Current Ammo: " + currentAmmo);
    }

}
