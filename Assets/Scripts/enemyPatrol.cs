using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyPatrol : MonoBehaviour
{
    public Transform player; // Reference to the player object
    private bool isPlayerDetected = false; // Flag to check if the player is detected
    bool onDamaged, isDead = false;

    public GameObject bullet, dustShoot;
    GameObject dustShootEffect;
    public Transform startBullet;


    public GameObject pointA, pointB, gun;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;

    new Collider2D collider;
    

    float hp = 100;
    public float speed;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        anim.SetBool("isMoving", true);

        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // Automatically move
        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else 
        {
            rb.velocity = new Vector2(-speed, 0);
        }



        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            flip();
            currentPoint = pointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            flip();
            currentPoint = pointB.transform;
        }



        // Detecting player and automatically shoot
        if (isPlayerDetected)
        {

            if (timer > 1f)
            {
                timer = 0;
                shooting();
                dustShootEffect = Instantiate(dustShoot, gun.transform.GetChild(2).transform.position, gun.transform.GetChild(2).transform.rotation);
                Destroy(dustShootEffect, 1f);
            }


            
            Destroy(dustShootEffect, 1f);
            rb.velocity = new Vector2(0, 0);
            anim.SetBool("isMoving", false);
        }
        else
        {
            anim.SetBool("isMoving", true);
            gun.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }


        // Animation when get hurt
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
            anim.SetBool("isDead", true);
            isPlayerDetected = false;
            rb.velocity = new Vector2(0, 0);
            GameObject gun = gameObject.transform.GetChild(1).gameObject;
            gun.SetActive(false);
            Destroy(gameObject, 1.5f);
        }
    }


    // Detecting Player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == player)
        {
            Debug.Log("Player detected!");
            isPlayerDetected = true;

        }

    }

    public void OnDamaged()
    {
        onDamaged = true;
        hp -= 5;
        Debug.Log(hp);


        if (hp <= 0)
        {
            
            isDead = true;
        }
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
        {
            onDamaged = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == player)
        {
            isPlayerDetected = false;

        }
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void shooting()
    {
        GameObject shoot = Instantiate(bullet, gun.transform.GetChild(0).position, gun.transform.GetChild(0).rotation);
        Destroy(shoot, 3f);
    }





    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }


}



