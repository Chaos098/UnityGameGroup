
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cat : MonoBehaviour
{

    Vector2 checkpoint;


    float dirX, dirY, moveSpeed = 3f, jumpforce = 5f, groundCheckRadius = 0.2f, hp = 100, timer;
    bool isGrounded, canDoubleJump, onDamaged;
    public bool isDead = false;
    public bool ClimbingAllowed { get; set; }


    Animator anim;
    Rigidbody2D rb;
    new Collider2D collider;
    public Transform groundCheck;

    public LayerMask groundLayer;
    public TextMeshProUGUI numberOfFistKits;

    public HP HP_Bar;

    public float recentHP, maxHP = 100;
    int numberOfFistAid = 0;


    public Vector3 respawnPoint;
    public LayerMask groundLayer;






    // Use this for initialization
    void Start()
    {

        numberOfFistKits.text = "0";
        checkpoint = transform.position;
        collider = GetComponent<Collider2D>();

        recentHP = maxHP;
        HP_Bar.updateHPBar(recentHP, maxHP);

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



        if (isDead)
        {
            StartCoroutine(DeadAnimation(0.2f));
            StartCoroutine(Respawn(1.4f));
        }
    }


    // public void OnDamaged()
    // {
    //     recentHP -= 5;
    //     Debug.Log(recentHP);
    //     HP_Bar.updateHPBar(recentHP, maxHP);
        
    //     onDamaged = true;

    //     if (recentHP <= 0)
    //     {
    //         isDead = true;
    //     }
    // }

    public void AddKits()
    {
        numberOfFistAid += 1;
        numberOfFistKits.text = numberOfFistAid.ToString(); 

    }

    public void Health()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            recentHP = 100;
            numberOfFistAid -= 1;
        }
    }

    public void UpdateCheckpoint(Vector2 pos)
    {
        checkpoint = pos;
    } 

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("EnemyBullet"))




    }


    public void OnDamaged(float Damage)
    {
        Debug.Log(hp);
        hp -= Damage;

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


    IEnumerator DeadAnimation(float seconds)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpforce);

        yield return new WaitForSeconds(seconds);

        collider.enabled = false;
    }

    IEnumerator Respawn(float seconds)
    {
        recentHP = 100;
        isDead = false;

        yield return new WaitForSeconds(seconds);

        HP_Bar.updateHPBar(recentHP, maxHP);
        rb.velocity = new Vector2(0, 0);
        transform.position = checkpoint;
        collider.enabled = true;
    }









}



