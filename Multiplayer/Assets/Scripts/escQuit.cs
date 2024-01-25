using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class escQuit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //pressing escape key at any time forces it back to the title page
        if (Input.GetKey (KeyCode.Escape)) {
            SceneManager.LoadScene("TitlePage"); 

            // resetting all static variables
            GameManager.gameStart = true;
            GameManager.questionAttempts = 0;
            DrillController.gemCount = 0;
            PlayerController.gasLevel = 0;
            SceneStartScript.gameLevel = 0;
        }
    }
}
