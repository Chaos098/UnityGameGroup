using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{

    float dirX, dirY, moveSpeed = 3f, jumpforce = 5f, groundCheckRadius = 0.2f, hp = 100;
    bool isGrounded, canDoubleJump, onDamaged, isDead = false;
    public bool ClimbingAllowed { get; set; }

    Vector2 checkpoint;
    Animator anim;
    Rigidbody2D rb;
    new Collider2D collider;
    public Transform groundCheck;
    public LayerMask groundLayer;





    // Use this for initialization
    void Start()
    {
        checkpoint = transform.position;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
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


        if (isDead)
        {

            StartCoroutine(DeadAnimation(0.4f));
            StartCoroutine(Respawn(1.4f));

        }
    }


    public void OnDamaged()
    {
        hp -= 20;
        Debug.Log(hp);
        onDamaged = true;

        if (hp <= 0)
        {
            isDead = true;
        }
    }

    public void UpdateCheckpoint(Vector2 pos)
    {
        checkpoint = pos;
    } 

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
        {
            onDamaged = false;
        }
    }

    IEnumerator DeadAnimation(float seconds)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpforce);

        yield return new WaitForSeconds(seconds);

        rb.velocity = new Vector2(rb.velocity.x, -10f);
        collider.enabled = false;


    }

    IEnumerator Respawn(float seconds)
    {
        hp = 100;
        isDead = false;


        yield return new WaitForSeconds(seconds);

        rb.velocity = new Vector2(0, 0);
        transform.position = checkpoint;
        collider.enabled = true;
    }







}
