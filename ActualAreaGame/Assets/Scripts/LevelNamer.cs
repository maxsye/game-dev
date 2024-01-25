using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelNamer : MonoBehaviour
{
    // Start is called before the first frame update
    public static string gameLevelName = "";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneStartScript.gameLevel == 0)
        {
            gameLevelName = "Future";
        }
        else if (SceneStartScript.gameLevel == 1)
        {
            gameLevelName = "Business";
        }
        else if (SceneStartScript.gameLevel == 2)
        {
            gameLevelName = "Leader";
        }
        else if (SceneStartScript.gameLevel == 3)
        {
            gameLevelName = "America";
        } else {
            SceneManager.LoadScene ("EndScreen");
            Debug.Log ("game over!");
            // switch to final 'game over' screen
        }
        gameObject.GetComponent<TextMeshProUGUI>().text = gameLevelName;
    }
}
