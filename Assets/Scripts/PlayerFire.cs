using UnityEngine;
using Fusion;

public class PlayerFire : NetworkBehaviour
{
    public CharacterData _characterData;
    public GameObject bulletPrefab; // Mermi prefab'ý
    public Transform firePoint; // Merminin fýrlatýlacaðý nokta
    float nextFireTime;


    private void Start()
    {
       
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
    }



    void Update()
    {

        RotateFirePointToMouse();

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + _characterData.FireRate;
            Shot();
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
    }


}
