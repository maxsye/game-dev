using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//The DrillController class controlls the behavior and movement of the Drill 
public class DrillController : MonoBehaviour
{
    //initial variables for setup
    public int numGems = 10;
    public GameObject gemPrefab;
    public GameObject groundPrefab;
    public GameObject acidGroundPrefab;
    public static int gemCount = 0;
    public Text scoreText;
    private float timeCounter = PlayerController.gasLevel * 10;
    Rigidbody2D rigidbody2d;
    public SimpleHealthBar gasBar;

    // Start is called before the first frame update
    void Start()
    {
        //sets initial values of the Underground scene and orients and positions the drill in its proper starting location
        gemCount = 0; 
        rigidbody2d = GetComponent<Rigidbody2D>();
        Vector2 position = transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        //Function calls to generate the ground and gems
        GenerateGround(54, 48);
        GenerateGems(numGems);

    }

    // Update is called once per frame
    void Update() {
        //Updates the time bar and switches the scene based on gemCount; if there are no gems collected,
        //then the game will revert to the GameScene, but if there are gems collected, then the game will
        //revert to the QuizGame scene where the number of gems will translate to the number of answer attempts.
        gasBar.UpdateBar (timeCounter, 50);
        scoreText.text = "Gems: " + gemCount;

        timeCounter =  (timeCounter - Time.deltaTime);

        if (timeCounter <= 0) {
            // switch scenes back to the main game
            Debug.Log("time up!!");
            if (gemCount > 0) {
                SceneManager.LoadScene ("QuizGame");
            } else {
                SceneManager.LoadScene ("GameScene");
            }
            
        }

        //the following code sets the controls for the Drill movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 position = rigidbody2d.position;
        if (Input.GetKey (KeyCode.D))
        {
            transform.Rotate(0, 0, -2f);
        }
        if (Input.GetKey (KeyCode.A))
        {
            transform.Rotate(0, 0, 2f);
        }
        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("forward");
            transform.Translate(Vector3.down * 3f * Time.deltaTime);
        }
        rigidbody2d.MovePosition(position);
    }
    
    //Generates numGems amount of gems randomly across the scene
    void GenerateGems(int numGems)
    {
        //Instantiate(gemPrefab, new Vector3(1, -23, 0), Quaternion.identity);
        string[] visited = new string[numGems];
        for (int i = 0; i < numGems; i++)
        {
            int posX = Random.Range(-27, 27);
            int posY = Random.Range(-24, 24);
            while (visited.Contains(posX + "," + posY))
            {
                posX = Random.Range(-27, 27);
                posY = Random.Range(-24, 24);
            }

            visited[i] = posX + "," + posY;

            Instantiate(gemPrefab, new Vector3(posX, posY, 0), Quaternion.identity);
        }
    }

    //generates ground of size w x l. 
    void GenerateGround (int w, int l)
    {
        for (int i = -(w/2); i < (w/2); i ++)
        {
            for (int j = -(l/2); j < (l/2); j++)
            {
                Instantiate(groundPrefab, new Vector3(i, j, 0), Quaternion.identity);
            }
        }


        //left vert rect
        for (int i = -(w/2)-10; i < -(w/2); i ++)
        {
            for (int j = -(l / 2)-10; j < (l / 2)+10; j++)
            {
                Instantiate(acidGroundPrefab, new Vector3(i, j, 0), Quaternion.identity);
            }
        }

        //right vert rect
        for (int i = (w / 2); i < (w / 2)+10; i++)
        {
            for (int j = -(l / 2) - 10; j < (l / 2)+10; j++)
            {
                Instantiate(acidGroundPrefab, new Vector3(i, j, 0), Quaternion.identity);
            }
        }

        //top horiz. rect
        for (int i = -(w / 2); i < (w / 2); i++)
        {
            for (int j = (l / 2); j < (l / 2)+10; j++)
            {
                Instantiate(acidGroundPrefab, new Vector3(i, j, 0), Quaternion.identity);
            }
        }

        //bottom horiz rect
        for (int i = -(w / 2); i < (w / 2); i++)
        {
            for (int j = -(l / 2) - 10; j < -(l / 2); j++)
            {
                Instantiate(acidGroundPrefab, new Vector3(i, j, 0), Quaternion.identity);
            }
        }
    }

    //Handles collision between drill and gems, destroying gems upon collision and adding 
    //to gemCount for every gem collected. 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "gem")
        {
            Destroy(collision.gameObject);
            gemCount++;
        }
        Debug.Log("Gems: " + gemCount);
        
    }

}
