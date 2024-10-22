using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;


    [SerializeField]
    GameObject dustShoot;
    GameObject dustShootEffect;

    Animator gunEffect;


    int totalWeapon;
    int[] amountBullets;
    public int currentIndexWeapon;


    public GameObject[] guns;
    public GameObject weaponHolder;
    public GameObject currentGun;


    void Start()
    {


        totalWeapon = weaponHolder.transform.childCount;
        guns = new GameObject[totalWeapon];
        amountBullets = new int[totalWeapon];

        for (int i = 1; i < totalWeapon; i++)
        {
            guns[i] = weaponHolder.transform.GetChild(i).gameObject;
            switch (guns[i].name)
            {
                case "Pistol":
                    amountBullets[i] = 12;
                    break;
                case "Riffle":
                    amountBullets[i] = 24;
                    break;
                case "Shotgun":
                    amountBullets[i] = 9;
                    break;
            }

            guns[i].SetActive(false);
        }

        guns[1].SetActive(true);
        currentGun = guns[1];
        currentIndexWeapon = 1;
    }




    // Update is called once per frame
    void Update()
    {

        // Flip player when run rotate
        FlipWeapon();


        // Add weapon into gunholder
        PickUp();


        //Take curren weapon animator
        gunEffect = currentGun.GetComponent<Animator>();



        // Change weapon
        SwapWeapon();



        // Shooting action
        if (Input.GetMouseButtonDown(0))
        {
            int index = 0;

            // Take amount of bullet of current gun
            for (int i = 1; i < totalWeapon; i++)
            {
                if (guns[i].name.Equals(currentGun.name))
                {
                    index = i;
                }
            }


            // If shoot bullet decrease
            if (amountBullets[index] == 0)
            {
                amountBullets[index] = 0;
            }
            else
            {
                amountBullets[index] -= 1;
            }


            //Debug.Log(amountBullets[index]);
            //Debug.Log(currentGun.name);


            // Run animation of shooting
            if (amountBullets[index] > 0)
            {
                shooting();
                gunEffect.SetBool("isShooting", true);

                //Take the Transform (GameObject) of "currentgun" of DustShoot
                dustShootEffect = Instantiate(dustShoot, currentGun.transform.GetChild(2).transform.position, currentGun.transform.GetChild(2).transform.rotation);
            }
        }
        else
        {
            gunEffect.SetBool("isShooting", false);

            Destroy(dustShootEffect, 1f);
        }


        Reload();


    }

    void PickUp()
    {
        if (weaponHolder.transform.childCount > totalWeapon)
        {
            totalWeapon = weaponHolder.transform.childCount;

            guns = new GameObject[totalWeapon];

            int[] tmpAmountBullets = new int[totalWeapon];

            for (int i = 1; i < totalWeapon - 1; i++)
            {
                tmpAmountBullets[i] = amountBullets[i];
            }



            for (int i = 1; i < totalWeapon; i++)
            {
                guns[i] = weaponHolder.transform.GetChild(i).gameObject;
                guns[i].SetActive(false);
            }
            switch (guns[totalWeapon - 1].name)
            {
                case "Pistol":
                    tmpAmountBullets[totalWeapon - 1] = 12;
                    break;
                case "Riffle":
                    tmpAmountBullets[totalWeapon - 1] = 24;
                    break;
                case "Shotgun":
                    tmpAmountBullets[totalWeapon - 1] = 9;
                    break;
            }

            amountBullets = new int[totalWeapon];

            for (int i = 1; i < totalWeapon; i++)
            {
                amountBullets[i] = tmpAmountBullets[i];
            }


            guns[currentIndexWeapon].SetActive(true);
            currentGun = guns[currentIndexWeapon];
        }
    }

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentGun.name == "Pistol")
        {
            for (int i = 1; i < totalWeapon; i++)
            {
                if (guns[i].name.Equals(currentGun.name))
                {
                    amountBullets[i] = 12;
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.R) && currentGun.name == "Riffle")
        {
            for (int i = 1; i < totalWeapon; i++)
            {
                if (guns[i].name.Equals(currentGun.name))
                {
                    amountBullets[i] = 24;
                }
            }
        }
    }
    



    void SwapWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentIndexWeapon < totalWeapon - 1)
            {
                guns[currentIndexWeapon].SetActive(false);
                currentIndexWeapon++;
                guns[currentIndexWeapon].SetActive(true);
                currentGun = guns[currentIndexWeapon];
            }
            else
            {
                guns[currentIndexWeapon].SetActive(false);
                currentIndexWeapon = 1;
                guns[currentIndexWeapon].SetActive(true);
                currentGun = guns[currentIndexWeapon];
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentIndexWeapon > 1)
            {
                guns[currentIndexWeapon].SetActive(false);
                currentIndexWeapon--;
                guns[currentIndexWeapon].SetActive(true);
                currentGun = guns[currentIndexWeapon];
            }
            else
            {
                guns[currentIndexWeapon].SetActive(false);
                currentIndexWeapon = totalWeapon - 1;
                guns[currentIndexWeapon].SetActive(true);
                currentGun = guns[currentIndexWeapon];
            }
        }
    }


    void FlipWeapon()
    {
        Vector3 gunpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (gunpos.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
        }
    }

    void shooting()
    {
        GameObject shoot = Instantiate(bullet, currentGun.transform.GetChild(0).transform.position, currentGun.transform.GetChild(0).transform.rotation);
        Destroy(shoot, 3f);
    }

    
}














