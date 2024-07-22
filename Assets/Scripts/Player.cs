using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    private void Start()
    {
        //KAMERA TAK�P ���N PHOTON KODU
        /*if (photonView.IsMine)
        {
            Camera.main.GetComponent<CameraFollow>().SetTarget(transform);
        }*/


        PlayerSprite.sprite = Character.CharacterImage;
        WeaponSprite.sprite = Character.WeaponImage;
        movementSpeed = Character.CharacterSpeed;
        jumpForce = Character.CharacterJumpForce;
        maxJumpCount = Character.characterJumpCount;
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // photonview kontrol�
        if (photonView.IsMine)
            {
            
        

        // Karakterin sola ve sa�a hareket etmesi
        float moveInput = Input.GetAxis("Horizontal");
        rb2d.velocity = new Vector2(moveInput * movementSpeed, rb2d.velocity.y);

        // Karakterin z�plamas� ve double jump
        if (Input.GetKeyDown(KeyCode.W) && jumpCount < maxJumpCount)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            jumpCount++;
        }

        if (Input.GetKeyDown(KeyCode.E) && Character.Skill != null && !isSkillOnCooldown)
        {
            
             switch (Character.Skill.SkillName)
            {
                case ("Inviciblety"):
                    Debug.Log("a");
                    StartCoroutine(Invincibility());
                    break;

                case ("Bazuka"):
                   
                    StartCoroutine(bazukaSpawn());
                    break;

                case ("MagicWall"):
                    
                    StartCoroutine(magicwall());
                    break;

                case ("Catapult"):
                    
                    StartCoroutine(catapult());
                    break;




                }
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

        // Karakterin y�n�n� de�i�tirme (sola ve sa�a bakma)
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Zemin kontrol�
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.5f, groundLayer);
        if (isGrounded)
        {
            jumpCount = 0; // Z�plama sayac�n� s�f�rlay�n
        }
    }

    private IEnumerator Invincibility()
    {

        isSkillOnCooldown = true;
        PlayerSprite.enabled = false;
        yield return new WaitForSeconds(Character.Skill.Duration);
        PlayerSprite.enabled = true;
        yield return new WaitForSeconds(Character.Skill.CoolDown);
        isSkillOnCooldown = false;
    }

    private IEnumerator bazukaSpawn()
    {
        isSkillOnCooldown = true;
        GameObject bazukaa = Instantiate(Character.Skill.ProjectilePrefeab,gameObject.transform.position,Quaternion.identity);
        yield return new WaitForSeconds(Character.Skill.CoolDown);
        isSkillOnCooldown = false;
    }

    private IEnumerator magicwall()
    {
        isSkillOnCooldown = true;
        Debug.Log("magic okeyss");
        yield return new WaitForSeconds(Character.Skill.CoolDown);
        isSkillOnCooldown = false;
    }

    private IEnumerator catapult()
    {
        isSkillOnCooldown = true;
        Debug.Log("cata okeyss");
        yield return new WaitForSeconds(Character.Skill.CoolDown);
        isSkillOnCooldown = false;
    }

    private IEnumerator DashCount()
    {
        yield return new WaitForSeconds(2);
        isDashing = false;
    }
}
