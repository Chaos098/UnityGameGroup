using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{

    float dirX, dirY, moveSpeed = 3f, jumpforce = 5f, groundCheckRadius = 0.2f, hp = 100, timer;
    bool isGrounded, canDoubleJump, onDamaged, isDead = false;
    public bool ClimbingAllowed { get; set; }

    Animator anim;
    Rigidbody2D rb;
    new Collider2D collider;
    public Transform groundCheck;
    public Vector3 respawnPoint;
    public LayerMask groundLayer;





    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        float moveInput = Input.GetAxisRaw("Horizontal");
        dirX = moveInput * moveSpeed;


        // Check if player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);


        // Player double jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
            canDoubleJump = true;
        }
        else if (canDoubleJump && Input.GetButtonDown("Jump"))
        {
            Jump();
            canDoubleJump = false;
        }


        //Player Climb
        if (ClimbingAllowed)
        {
            dirY = Input.GetAxisRaw("Vertical") * moveSpeed * 1.5f;
        }



        SetAnimationState();


        // Player dead animation
        if (isDead)
        {
            if (timer > 1f)
            {
                rb.velocity = new Vector2(rb.velocity.x, 7f);
                timer = 0;
            }


            collider.enabled = false;
            rb.gravityScale = 3f;

            //StartCoroutine(Respawn(1f));
            
        }



    }



    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX, rb.velocity.y);


        // Player climb
        if (ClimbingAllowed)
        {
            rb.isKinematic = true;
            rb.velocity = new Vector2(dirX, dirY);
        }
        else
        {
            rb.isKinematic = false;
            rb.velocity = new Vector2(dirX, rb.velocity.y);
        }
    }


    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpforce);
    }


    void SetAnimationState()
    {
        if (dirX == 0)
        {
            anim.SetBool("isMoving", false);
        }

        if (Mathf.Abs(dirX) == 3 && rb.velocity.y == 0)
        {
            anim.SetBool("isMoving", true);
        }

        if (dirY == 0 || isGrounded)
        {
            anim.SetBool("isClimbing", false);
        }

        if (Mathf.Abs(dirY) == 4.5 && rb.velocity.x == 0 )
        {
            anim.SetBool("isClimbing", true);
        }

        if (onDamaged)
        {
            anim.SetBool("isHurt", true);
        }

        if (!onDamaged)
        {
            anim.SetBool("isHurt", false);
        }






    }


    public void OnDamaged()
    {
        Debug.Log(hp);
        hp -= 20;

        if (hp <= 0)
        {
            isDead = true;
        }
        onDamaged = true;
    }

    //IEnumerator Disabled(float duration)
    //{
    //    // Wait for 2 seconds
    //    yield return new WaitForSeconds(duration);
    //    gameObject.SetActive(false);

    //}

    //IEnumerator Respawn(float duration)
    //{
    //    hp = 100;
    //    // Wait for 2 seconds
    //    yield return new WaitForSeconds(duration);
    //    //gameObject.SetActive(true);
    //    transform.position = respawnPoint;
    //    rb.velocity = new Vector2(0, 0);
    //    collider.enabled = true;
    //    rb.gravityScale = 1.5f;


    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
        {
            onDamaged = false;
        }
    }








}
