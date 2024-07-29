using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPun
{
    public SpriteRenderer PlayerSprite;
    public SpriteRenderer WeaponSprite;
    public int movementSpeed;
    public int jumpForce;
    public CharacterData Character;
    private int jumpCount;
    private int maxJumpCount;
    private Rigidbody2D rb2d;
    public LayerMask groundLayer;
    public bool isGrounded;
    private bool isDashing;
    private bool isSkillOnCooldown;
    public Transform firePoint;

    private void Start()
    {
        // Ba�lang�� ayarlar�
        GameObject fp = GameObject.FindGameObjectWithTag("FirePoint");
        firePoint = fp.transform;
        PlayerSprite.sprite = Character.CharacterImage;
        WeaponSprite.sprite = Character.WeaponImage;
        movementSpeed = Character.CharacterSpeed;
        jumpForce = Character.CharacterJumpForce;
        maxJumpCount = Character.characterJumpCount;
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // Yatay hareket
            rb2d.velocity = new Vector2(Input.GetAxis("Horizontal") * movementSpeed, rb2d.velocity.y);

            // Z�plama ve �ift z�plama
            if (Input.GetKeyDown(KeyCode.W) && jumpCount < maxJumpCount)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                jumpCount++;
            }

            // Skill kullan�m�
            if (Input.GetKeyDown(KeyCode.E) && Character.Skill != null && !isSkillOnCooldown)
            {
                UseSkill();
            }

            // Dash
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
            {
                StartCoroutine(DashCount());
            }

            // Y�n de�i�tirme
            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            // Zemin kontrol�
            isGrounded = Physics2D.OverlapCircle(transform.position, 0.5f, groundLayer);
            if (isGrounded)
            {
                jumpCount = 0; // Z�plama s�f�rlama
            }
        }
    }

    private void UseSkill()
    {
        switch (Character.Skill.SkillName)
        {
            case "Inviciblety":
                StartCoroutine(Invincibility());
                break;
            case "Bazuka":
                StartCoroutine(BazukaSpawn());
                break;
            case "MagicWall":
                StartCoroutine(MagicWall());
                break;
            case "Catapult":
                StartCoroutine(Catapult());
                break;
        }
    }

    private IEnumerator Invincibility()
    {
        photonView.RPC("SetInvincibility", RpcTarget.OthersBuffered, true);
        PlayerSprite.material.color = new Color(1, 1, 1, 0.2f);
        yield return new WaitForSeconds(Character.Skill.Duration);
        photonView.RPC("SetInvincibility", RpcTarget.OthersBuffered, false);
        PlayerSprite.material.color = new Color(1, 1, 1, 1f);
        yield return new WaitForSeconds(Character.Skill.CoolDown);
        isSkillOnCooldown = false;
    }

    [PunRPC]
    void SetInvincibility(bool isInvincible)
    {
        isSkillOnCooldown = isInvincible;
        PlayerSprite.enabled = !isInvincible;
        WeaponSprite.enabled = !isInvincible;
    }

    private IEnumerator BazukaSpawn()
    {
        isSkillOnCooldown = true;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;
        mousePosition.z = 0f;

        photonView.RPC("SpawnBazuka", RpcTarget.All, transform.position, firePoint.rotation, direction);
        yield return new WaitForSeconds(Character.Skill.CoolDown);
        isSkillOnCooldown = false;
    }

    [PunRPC]
    void SpawnBazuka(Vector3 position, Quaternion rotation, Vector2 direction)
    {
        GameObject bazuka = PhotonNetwork.Instantiate(Character.Skill.ProjectilePrefeab.name, position, rotation);
        bazuka.GetComponent<Rigidbody2D>().velocity = direction * Character.Skill.ProjectileSpeed;
    }

    private IEnumerator MagicWall()
    {
        isSkillOnCooldown = true;
        photonView.RPC("SpawnMagicWall", RpcTarget.All, transform.position);
        yield return new WaitForSeconds(Character.Skill.CoolDown);
        isSkillOnCooldown = false;
    }

    [PunRPC]
    void SpawnMagicWall(Vector3 position)
    {
        StartCoroutine(MagicWallCoroutine(position));
        // Magic Wall olu�turma mekani�i YAZILACAK
        Debug.Log("Magic Wall spawned at " + position);
    }

    private IEnumerator MagicWallCoroutine(Vector3 position)
    {
        GameObject wall = PhotonNetwork.Instantiate("MagicWallPrefab", position, Quaternion.identity);
        Rigidbody2D wallRb = wall.GetComponent<Rigidbody2D>();
        if (wallRb != null)
        {
            wallRb.velocity = new Vector2(10f, 0f); // Duvar�n h�z�n� ayarlay�n
        }

        Vector3 initialScale = wall.transform.localScale;
        Vector3 targetScale = new Vector3(2f, 5f, 0f); // B�y�me hedefi

        float growthDuration = 1f; // B�y�me s�resi
        float elapsedTime = 0f;

        while (elapsedTime < growthDuration)
        {
            wall.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / growthDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Duvar� yar�m ay �ekline d�n��t�rme
        float halfMoonDuration = 1f; // Yar�m ay olma s�resi
        elapsedTime = 0f;
        Vector3 finalScale = new Vector3(0f, targetScale.y, targetScale.z);

        while (elapsedTime < halfMoonDuration)
        {
            wall.transform.localScale = Vector3.Lerp(targetScale, finalScale, elapsedTime / halfMoonDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        PhotonNetwork.Destroy(wall); // Duvar� yok et
    }

    private IEnumerator Catapult()
    {
        isSkillOnCooldown = true;
        photonView.RPC("SpawnCatapult", RpcTarget.All, firePoint.position);
        yield return new WaitForSeconds(Character.Skill.CoolDown);
        isSkillOnCooldown = false;
    }

    [PunRPC]
    void SpawnCatapult(Vector3 position, int targetViewID)
    {
        StartCoroutine(CatapultCoroutine(targetViewID));
        Debug.Log("Catapult activated at " + position);
    }

    private IEnumerator CatapultCoroutine(int targetViewID)
    {
        for (int i = 0; i < 3; i++)
        {
            // Hedef oyuncunun PhotonView'ini bul
            PhotonView targetView = PhotonView.Find(targetViewID);
            if (targetView != null)
            {
                // Hedef oyuncunun anl�k pozisyonunu al
                Vector3 playerPosition = targetView.transform.position;

                // Spawn pozisyonunu hesapla (oyuncunun �st�nden)
                Vector3 spawnPosition = new Vector3(playerPosition.x, playerPosition.y + 10f, 0f);

                // Nesneyi spawnla
                GameObject fallingObject = PhotonNetwork.Instantiate("FallingObjectPrefab", spawnPosition, Quaternion.identity);

                // A�a��ya d��me mekani�i
                Rigidbody2D rb = fallingObject.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.gravityScale = 3f;
                }
            }

            // Yar�m saniye bekle
            yield return new WaitForSeconds(0.3f);
        }
    }

    // Kullan�c� skilli kulland���nda �a�r�l�r
    public void UseCatapultSkill(int targetViewID)
    {
        StartCoroutine(CatapultCoroutine(targetViewID));
    }





    private IEnumerator DashCount()
    {
        photonView.RPC("SetDash", RpcTarget.AllBuffered, true);
        yield return new WaitForSeconds(2);
        photonView.RPC("SetDash", RpcTarget.AllBuffered, false);
        isDashing = false;
    }

    [PunRPC]
    void SetDash(bool isDash)
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.position += new Vector3(2, 0, 0);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.position += new Vector3(-2, 0, 0);
        }
        isDashing = true;
    }
}
