using Photon.Pun;
using UnityEngine;

public class PlayerFire : MonoBehaviourPun
{
    public CharacterData _characterData;
    public GameObject bulletPrefab; // Mermi prefab'�
    public Transform firePoint; // Merminin f�rlat�laca�� nokta
    float nextFireTime;

    private void Start()
    {
        _characterData = gameObject.GetComponent<Player>().Character;
        GameObject fp = GameObject.FindGameObjectWithTag("FirePoint");
        firePoint = fp.transform;
        bulletPrefab = _characterData.projectilePrefab;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            RotateFirePointToMouse();

            if (Input.GetMouseButton(0) && Time.time >= nextFireTime) // Sol fare tu�u ile ate� etme
            {
                nextFireTime = Time.time + _characterData.FireRate;
                Shot();
            }
        }
    }

    void RotateFirePointToMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Z eksenini s�f�rla ��nk� 2D oyunda Z ekseni kullan�lmaz

        // Oyuncudan fareye olan y�n� hesapla
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Fire Point'in yeni pozisyonunu belirle
        firePoint.position = transform.position + (Vector3)direction;

        // Fire Point'i bu y�ne d�nd�r
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Shot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Z eksenini s�f�rla ��nk� 2D oyunda Z ekseni kullan�lmaz

        // Ate� etme y�n�n� belirle
        Vector2 direction = (mousePosition - firePoint.position).normalized;

        // Mermiyi olu�tur ve RPC ile ate� etme bilgisini t�m oyunculara g�nder
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation);
        photonView.RPC("ShootFire", RpcTarget.All, bullet.GetPhotonView().ViewID, direction, _characterData.FireSpeed, _characterData.KnockBackRate);
    }

    [PunRPC]
    void ShootFire(int bulletViewID, Vector2 dir, float fireSpeed, float knockBackRate)
    {
        GameObject bullet = PhotonView.Find(bulletViewID).gameObject;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        mermi mer = bullet.GetComponent<mermi>();
        rb.velocity = dir * fireSpeed; // Mermiyi belirlenen y�ne do�ru f�rlatmak i�in
    }
}
