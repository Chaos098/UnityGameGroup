using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletItem : MonoBehaviour
{
    public GameObject gunholder;
    public Transform pos;
    Animator anim;



    void Start()
    {
        anim = GetComponent<Animator>();

    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Item picked");
            collision.SendMessageUpwards("AddBullet", gameObject.name);
            Destroy(gameObject);

        }

        if (collision.CompareTag("Player") && !Input.GetKeyDown(KeyCode.F))
        {
            anim.SetBool("isTouching", true);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        anim.SetBool("isTouching", false);
    }
}
