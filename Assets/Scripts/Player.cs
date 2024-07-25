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
        // Baþlangýç ayarlarý
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

            // Zýplama ve çift zýplama
            if (Input.GetKeyDown(KeyCode.W) && jumpCount < maxJumpCount)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                jumpCount++;
            }

            // Skill kullanýmý
            if (Input.GetKeyDown(KeyCode.E) && Character.Skill != null && !isSkillOnCooldown)
            {
                UseSkill();
            }

            // Dash
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
            {
                StartCoroutine(DashCount());
            }

            // Yön deðiþtirme
            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            // Zemin kontrolü
            isGrounded = Physics2D.OverlapCircle(transform.position, 0.5f, groundLayer);
            if (isGrounded)
            {
                jumpCount = 0; // Zýplama sýfýrlama
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
        photonView.RPC("SetInvincibility", RpcTarget.AllBuffered, true);
        PlayerSprite.material.color = new Color(1, 1, 1, 0.2f);
        yield return new WaitForSeconds(Character.Skill.Duration);
        photonView.RPC("SetInvincibility", RpcTarget.AllBuffered, false);
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
        // Magic Wall oluþturma mekaniði YAZILACAK
        Debug.Log("Magic Wall spawned at " + position);
    }

    private IEnumerator Catapult()
    {
        isSkillOnCooldown = true;
        photonView.RPC("SpawnCatapult", RpcTarget.All, transform.position);
        yield return new WaitForSeconds(Character.Skill.CoolDown);
        isSkillOnCooldown = false;
    }

    [PunRPC]
    void SpawnCatapult(Vector3 position)
    {
        // Catapult mekaniði YAZILACAK
        Debug.Log("Catapult spawned at " + position);
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
