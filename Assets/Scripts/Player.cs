using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
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

    private void Start()
    {
        PlayerSprite.sprite = Character.CharacterImage;
        WeaponSprite.sprite = Character.WeaponImage;
        movementSpeed = Character.CharacterSpeed;
        jumpForce = Character.CharacterJumpForce;
        maxJumpCount = Character.characterJumpCount;
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Karakterin sola ve saða hareket etmesi
        float moveInput = Input.GetAxis("Horizontal");
        rb2d.velocity = new Vector2(moveInput * movementSpeed, rb2d.velocity.y);

        // Karakterin zýplamasý ve double jump
        if (Input.GetKeyDown(KeyCode.W) && jumpCount < maxJumpCount)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            jumpCount++;
        }

        if (Input.GetKeyDown(KeyCode.E) && Character._activeSkill != null && !isSkillOnCooldown)
        {
           // Character._activeSkill.Activate(gameObject);
            

            switch (Character._activeSkill)
            {
                case Inviciblety:
                    Debug.Log("a");
                    StartCoroutine(TempInvincibility((Inviciblety)Character._activeSkill));
                    break;

                case Bazuka:
                    Debug.Log("a");
                    StartCoroutine(bazukaSpawn((Bazuka)Character._activeSkill));
                    break;

                case MagicWall :
                    Debug.Log("c");
                    StartCoroutine(magicwall((MagicWall)Character._activeSkill));
                    break;

                case Catapult:
                    Debug.Log("d");
                    StartCoroutine(catapult((Catapult)Character._activeSkill));
                    break;




            }
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            if (moveInput > 0)
            {
                transform.position += new Vector3(2, 0, 0);
            }
            else if (moveInput < 0)
            {
                transform.position += new Vector3(-2, 0, 0);
            }
            isDashing = true;
            StartCoroutine(DashCount());
        }

        // Karakterin yönünü deðiþtirme (sola ve saða bakma)
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Zemin kontrolü
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.5f, groundLayer);
        if (isGrounded)
        {
            jumpCount = 0; // Zýplama sayacýný sýfýrlayýn
        }
    }

    private IEnumerator TempInvincibility(Inviciblety skill)
    {
        isSkillOnCooldown = true;
        PlayerSprite.enabled = false;
        yield return new WaitForSeconds(skill.duration);
        PlayerSprite.enabled = true;
        yield return new WaitForSeconds(skill.cooldown);
        isSkillOnCooldown = false;
    }

    private IEnumerator bazukaSpawn(Bazuka Skill)
    {
        isSkillOnCooldown = true;
        GameObject bazukaa = Instantiate(Skill.fireballPrefab);
        Debug.Log("spawn");
        yield return new WaitForSeconds(Skill.cooldown);
        isSkillOnCooldown = false;
    }

    private IEnumerator magicwall(MagicWall Skill)
    {
        isSkillOnCooldown = true;
        Debug.Log("magic okeyss");
        yield return new WaitForSeconds(Skill.cooldown);
        isSkillOnCooldown = false;
    }

    private IEnumerator catapult(Catapult Skill)
    {
        isSkillOnCooldown = true;
        Debug.Log("cata okeyss");
        yield return new WaitForSeconds(Skill.cooldown);
        isSkillOnCooldown = false;
    }

    private IEnumerator DashCount()
    {
        yield return new WaitForSeconds(2);
        isDashing = false;
    }
}
