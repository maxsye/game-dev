using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinalMessage : MonoBehaviour
{

    public TextMeshProUGUI awardText;
    private int attempts = GameManager.questionAttempts;

    // Start is called before the first frame update
    void Start()
    {
        attempts = GameManager.questionAttempts;
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "CONGRATULATIONS BAADriller!!!\nYou finished with " + attempts + " attempts!"; //tells them how many attempts they had
        if (attempts < 40) //determines what award to give based on the number of attempts
        { //if you want them to obtain America at lower or higher amount of attempts, you can change it here
            awardText.GetComponent<TMPro.TextMeshProUGUI>().text = "America";
        }
        else if (attempts < 50)
        { 
            awardText.GetComponent<TMPro.TextMeshProUGUI>().text = "Leader";
        }
        else if (attempts < 60)
        { 
            awardText.GetComponent<TMPro.TextMeshProUGUI>().text = "Business";
        }
        else
        { 
            awardText.GetComponent<TMPro.TextMeshProUGUI>().text = "Future";
        }
    }
    

    // Update is called once per frame
    void Update()
    {

    }
}
