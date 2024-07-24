using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
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
                photonView.RPC("Shoot", RpcTarget.AllBuffered);
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

    

    [PunRPC]
    void Shoot()
    {
        Debug.Log("a");
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // Z eksenini s�f�rla ��nk� 2D oyunda Z ekseni kullan�lmaz

            // Ate� etme y�n�n� belirle
            Vector2 direction = (mousePosition - firePoint.position).normalized;

            // Mermiyi olu�tur
            GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation);
            bullet.GetComponent<mermi>()._cD = _characterData;


            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = direction * _characterData.FireSpeed; // Mermiyi belirlenen y�ne do�ru f�rlat
             
    }
}
