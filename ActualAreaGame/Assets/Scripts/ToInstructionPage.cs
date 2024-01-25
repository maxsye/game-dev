using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//loads the instruction page
//if you want this to go to a different page, change
//SceneManager.LoadScene("Instructions") to SceneManager.LoadScene("SceneName")

public class ToInstructionPage : MonoBehaviour
{
    public void toInstructionPage () {
        SceneManager.LoadScene("InstructionPage");
    }

    public void quitGame () {
        Application.Quit ();
    }
    
    public void toOptionsPage () {
        SceneManager.LoadScene("Options");
    }
}
