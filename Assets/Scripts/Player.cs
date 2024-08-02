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
    public bool isSkillOnCooldown;
    public Transform firePoint;

    public GameObject pop;

    private void Start()
    {
        GameObject fp = GameObject.FindGameObjectWithTag("FirePoint");
        firePoint = fp.transform;
        PlayerSprite.sprite = Character.InGamePlayer;
        WeaponSprite.sprite = Character.InGameWeapon;
        movementSpeed = Character.CharacterSpeed;
        jumpForce = Character.CharacterJumpForce;
        maxJumpCount = Character.characterJumpCount;
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            // Yön deðiþtirme
            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

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

            // Zemin kontrolü
            isGrounded = Physics2D.OverlapCircle(transform.position, 0.5f, groundLayer);
            if (isGrounded)
            {
                jumpCount = 0; // Zýplama sýfýrlama
            }
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            // Yatay hareket
            rb2d.velocity = new Vector2(Input.GetAxis("Horizontal") * movementSpeed, rb2d.velocity.y);
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

        GameObject bazuka = PhotonNetwork.Instantiate(Character.Skill.ProjectilePrefeab.name, firePoint.position, firePoint.rotation);
        photonView.RPC("SpawnBazuka", RpcTarget.All, bazuka.GetPhotonView().ViewID, direction);
        yield return new WaitForSeconds(Character.Skill.CoolDown);
        isSkillOnCooldown = false;
    }

    [PunRPC]
    void SpawnBazuka(int bazukaID, Vector2 direction)
    {
        GameObject bazuka = PhotonView.Find(bazukaID).gameObject;
        Rigidbody2D rb = bazuka.GetComponent<Rigidbody2D>();
        bazuka.GetComponent<Rigidbody2D>().velocity = direction * Character.Skill.ProjectileSpeed;
    }

    private IEnumerator MagicWall()
    {
        isSkillOnCooldown = true;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 direction = (mousePosition - firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion wallRotation = Quaternion.Euler(new Vector3(0, 0, angle + 130));

        GameObject Wall = PhotonNetwork.Instantiate(Character.Skill.ProjectilePrefeab.name, firePoint.position, wallRotation);
        StartCoroutine(ScaleAndDestroyWall(Wall));
        photonView.RPC("SpawnMagicWall", RpcTarget.All, Wall.GetPhotonView().ViewID, direction);
        yield return new WaitForSeconds(Character.Skill.CoolDown);
        isSkillOnCooldown = false;
    }

    [PunRPC]
    void SpawnMagicWall(int WallID, Vector2 direction)
    {
        GameObject wall = PhotonView.Find(WallID).gameObject;
        Rigidbody2D rb = wall.GetComponent<Rigidbody2D>();
        rb.velocity = direction * Character.Skill.ProjectileSpeed;
    }

    private IEnumerator ScaleAndDestroyWall(GameObject wall)
    {
        Vector3 initialScale = new Vector3(0.1f, 0.1f, 1f);
        Vector3 targetScale = new Vector3(0.5f, 0.5f, 1f);
        float growthDuration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < growthDuration)
        {
            wall.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / growthDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        PhotonNetwork.Destroy(wall);
    }

    private IEnumerator Catapult()
    {
        isSkillOnCooldown = true;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int targetViewID = -1;

        foreach (GameObject player in players)
        {
            PhotonView pv = player.GetComponent<PhotonView>();
            if (pv != null && !pv.IsMine)
            {
                targetViewID = pv.ViewID;
                break;
            }
        }

        if (targetViewID != -1)
        {
            photonView.RPC("CatapultCoroutine", RpcTarget.All, targetViewID);
        }

        yield return new WaitForSeconds(Character.Skill.CoolDown);
        isSkillOnCooldown = false;
    }

    [PunRPC]
    private IEnumerator CatapultCoroutine(int targetViewID)
    {
        for (int i = 0; i < 3; i++)
        {
            PhotonView targetView = PhotonView.Find(targetViewID);
            if (targetView != null)
            {
                Vector3 playerPosition = targetView.transform.position;
                Vector3 spawnPosition = new Vector3(playerPosition.x, playerPosition.y + 10f, 0f);

                GameObject fallingObject = Instantiate(pop, spawnPosition, Quaternion.identity);
                Rigidbody2D rb = fallingObject.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.gravityScale = 1f;
                }
            }
            yield return new WaitForSeconds(5f);
        }
    }

    private IEnumerator DashCount()
    {
        photonView.RPC("SetDash", RpcTarget.All, true);
        yield return new WaitForSeconds(2);
        photonView.RPC("SetDash", RpcTarget.All, false);
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
