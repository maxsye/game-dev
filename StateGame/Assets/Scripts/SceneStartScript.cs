using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneStartScript : MonoBehaviour
{
    public static int gameLevel = 0;
    public static int difficulty = 0;
    public void Switch()
    {
        GameManager.gameStart = true;
        GameManager.questionAttempts = 0;
        DrillController.gemCount = 0;
        PlayerController.gasLevel = 0;
        SceneStartScript.gameLevel = 0;
        SceneManager.LoadScene("GameScene");
    }
    public void easy()
    {
        difficulty = 0;
    }
    public void medium()
    {
        difficulty = 1;
    }
    public void hard()
    {
        difficulty = 2;
    }
}
