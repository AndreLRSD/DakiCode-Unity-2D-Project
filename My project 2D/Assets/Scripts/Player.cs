using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 3;
    public float speed;
    public float jumpforce;
    private bool isJumping;
    private bool doubleJumping;
    private bool isFire;
    public GameObject bow;
    public Transform Firepoint;

    private Rigidbody2D rig;
    private Animator anim;

    private float movement;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        Gamecontrol.instance.Updatelives(health);
    }

    // Update is called once per frame
    void Update()
    {
        jump();
        bowfire();
    }

    private void FixedUpdate()
    {
        move();
    }
    void move()
    {
        movement = Input.GetAxis("Horizontal");

        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        if(movement > 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("Transition", 1);
            }
            transform.eulerAngles = new Vector3(0,0,0);
        }

        if(movement < 0)
        {
            if (!isJumping)
            { 
            anim.SetInteger("Transition", 1);
            }
            transform.eulerAngles = new Vector3(0,180, 0);
        }

        if(movement == 0 && !isJumping && !isFire)
        {
            anim.SetInteger("Transition", 0);
        }
    }

    void jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                anim.SetInteger("Transition", 2);
                rig.AddForce(new Vector2(0, jumpforce), ForceMode2D.Impulse);
                doubleJumping = true;
                isJumping = true;
            }
            else
            {
                if (doubleJumping)
                {
                    anim.SetInteger("Transition", 2);
                    rig.AddForce(new Vector2(0, jumpforce * 1), ForceMode2D.Impulse);
                    doubleJumping = false;
                }
            }
        }
    }

    void bowfire()
    {
        StartCoroutine("Fire");
    }

    IEnumerator Fire()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isFire = true;
            anim.SetInteger("Transition", 3);
            GameObject Bow = Instantiate(bow, Firepoint.position, Firepoint.rotation);
            if(transform.rotation.y == 0)
            {
                Bow.GetComponent<Bow>().isRight = true;
            }
            if (transform.rotation.y == 180)
            {
                Bow.GetComponent<Bow>().isRight = false;
            }

            yield return new WaitForSeconds(0.2f);
            isFire = false;
            anim.SetInteger("Transition", 0);
        }
    }

    public void Damage(int dmg)
    {
        health -= dmg;
        Gamecontrol.instance.Updatelives(health);
        anim.SetTrigger("Hit");
        if (transform.rotation.y == 0)
        {
            transform.position += new Vector3(-0.5f, 0, 0);
        }
        if (transform.rotation.y == 180)
        {
            transform.position += new Vector3(-0.5f, 0, 0);
        }

        if (health <= 0)
        {
            Gamecontrol.instance.GameOver();
        }
    }

    public void IncreaseLife(int value)
    {
        health += value;
        Gamecontrol.instance.Updatelives(health);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.layer == 8)
        {
            isJumping = false;
        }
        if (coll.gameObject.layer == 9)
        {
            Gamecontrol.instance.GameOver();
        }
    }
}
