using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCan : MonoBehaviour
{

    Animator anim;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 3 + (SceneStartScript.gameLevel)*3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startExplosion ()
    {
        anim.SetBool("explode", true);
        StartCoroutine(explode());
    }

    IEnumerator explode ()
    {
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "Player")
        {
            anim.SetBool("explode", true);
            StartCoroutine(explode());
        }
    }
}
