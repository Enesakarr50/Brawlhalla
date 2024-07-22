using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (Input.GetKeyDown(KeyCode.E) && Character._activeSkill != null)
        {
            Character._activeSkill.Activate(gameObject);
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing) // Varsayýlan Fire3 tuþu sol Shift'tir
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
    public IEnumerator dashCount()
    {
        yield return new WaitForSeconds(2);
        isDashing = false;
    }
}


