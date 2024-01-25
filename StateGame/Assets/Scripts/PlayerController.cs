using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public CharacterController controller;
    public Rigidbody2D rb;
    public float speed = 10f;
    float move = 0f;
    public Camera camera;
    private Animator anim;
    private float offset = 20f;
    public bool isGrounded;
    public float jumpVel = 5;
    public float fallMultiplier = 3f;
    public float lowMultiplier = 2f;
    public GameObject fireProj;
    public float bulletSpeed = 30f;
    private float lastShotTime;
    private int lives = 3;
    public GameObject life3;
    public GameObject life2;
    public GameObject life1;
    public static int gasLevel = 0;
    public SimpleHealthBar healthBar;
    public Text fullText;

    void Start()
    {
        anim = GetComponent<Animator>();
        gasLevel = 0;

        life1.SetActive (false);
        life2.SetActive (false);
        life3.SetActive (false);

        Debug.Log (SceneStartScript.difficulty);

        if (SceneStartScript.difficulty == 0)
        {
            lives = 3;
            life1.SetActive (true);
            life2.SetActive (true);
            life3.SetActive (true);
        }
        else if (SceneStartScript.difficulty == 1)
        {
            lives = 2;
            life1.SetActive (true);
            life2.SetActive (true);
            life3.SetActive(false);
        }
        else if (SceneStartScript.difficulty == 2)
        {
            lives = 1;
            life1.SetActive(false);
            life2.SetActive(false);
            life3.SetActive (true);
        }
    }


    // Update is called once per frame
    void Update()
    {
        healthBar.UpdateBar(gasLevel, 5);
        if (gasLevel >= 5)
        {
            fullText.enabled = true;
            gasLevel = 5;
        } else
        {
            fullText.enabled = false;
        }
        //gasText.GetComponent<UnityEngine.UI.Text>().text = "Gas Level: " + gasLevel;
        if (lives == 2)
        {
            life3.SetActive(false);
        } else if (lives == 1)
        {
            life2.SetActive(false);
        }
        else if (lives == 0)
        {
            life1.SetActive(false);
            //game over implemented here? or some kind of respawn for the level
            gasLevel = 0;
            SceneManager.LoadScene ("GameScene");
        }
        if (Input.GetKey (KeyCode.A))
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
            move = 1;
            anim.SetBool("run", true);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("run", false);
            move = 0;

        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            move = 1;
            anim.SetBool("run", true);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            
            anim.SetBool("run", false);
            move = 0;
        }
        if (Input.GetKeyDown(KeyCode.W) && rb.velocity.y == 0)
        {
            rb.velocity = Vector2.up * jumpVel;
        }
        if (anim.GetBool("run") == false)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            if (mousePos.x <= transform.position.x)
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
        }
        if (Time.time - lastShotTime >= .25f)
        {
            anim.SetBool("shooting", false);
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
                Debug.Log(target);
                Vector3 diff = target - transform.position;
                float dist = diff.magnitude;
                Vector2 dir = diff / dist;
                dir.Normalize();
                float rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                if (transform.rotation.eulerAngles.y == 0)
                {
                    if (target.x >= transform.position.x)
                    {
                        lastShotTime = Time.time;
                        GameObject bullet = Instantiate(fireProj, new Vector3(transform.position.x + 3.1f, transform.position.y + 2, 0), Quaternion.identity);
                        bullet.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
                        bullet.GetComponent<Rigidbody2D>().AddForce(dir * 2000);
                        anim.SetBool("shooting", true);
                    }
                }
                else
                {
                    if (target.x <= transform.position.x)
                    {
                        lastShotTime = Time.time;
                        GameObject bullet = Instantiate(fireProj, new Vector3(transform.position.x - 4.1f, transform.position.y + 1f, 0), Quaternion.identity);
                        bullet.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
                        bullet.GetComponent<Rigidbody2D>().AddForce(dir * 2000);
                        anim.SetBool("shooting", true);
                    }
                }
            }
        }
        

        if (rb.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
        }
        else if (rb.velocity.y == 0)
        {
            anim.SetBool("jumping", false);
        }

    }
    private void FixedUpdate()
    {
        transform.Translate(new Vector2(move * speed * Time.deltaTime, 0));
        if (transform.position.x - camera.transform.position.x >= offset && camera.transform.position.x <= 115)
        {
            camera.transform.position = new Vector3(transform.position.x - offset, camera.transform.position.y, camera.transform.position.z);
        }
        if (camera.transform.position.x - transform.position.x >= offset && camera.transform.position.x >= -75)
        {
            camera.transform.position = new Vector3(transform.position.x + offset, camera.transform.position.y, camera.transform.position.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "gascan")
        {
            lives -= 1;
        }
    }


}
