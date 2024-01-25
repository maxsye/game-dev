using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GenerateBars : MonoBehaviour
{
    public GameObject barPrefab;
    public SimpleHealthBar firstPlayer; 
    public SimpleHealthBar secondPlayer; 
    
    
    void Start()
    {
        if(SyncScore.scoreKeeper==null)
        {

            SyncScore.scoreKeeper = gameObject.AddComponent<SyncScore>();
        }
       // Debug.Log("scorekeeper instantiated" + SyncScore.scoreKeeper.player2Score);

        var player1 = Instantiate(barPrefab, new Vector3(-380, -290, 0), Quaternion.identity);
        player1.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        
        firstPlayer = player1.GetComponentInChildren<SimpleHealthBar>();
        
        var p1 = Instantiate(new GameObject("P1"), new Vector3(-512, -322, 0), Quaternion.identity);
        p1.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        p1.AddComponent<Text>().text = "P1";
        p1.gameObject.GetComponent<Text>().fontSize = 30;
        p1.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
    
        var player2 = Instantiate(barPrefab, new Vector3(400, -290, 0), Quaternion.identity);
        player2.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);

        secondPlayer = player2.GetComponentInChildren<SimpleHealthBar>();
        Debug.Log("Second Player: " + secondPlayer);

        var p2 = Instantiate(new GameObject("P2"), new Vector3(265, -322, 0), Quaternion.identity);
        p2.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);

        p2.AddComponent<Text>().text = "P2";
        p2.gameObject.GetComponent<Text>().fontSize = 30;
        p2.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        //player2.gameObject.transform.localScale -= new Vector3(1, 2, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("AFTERRRRRR");
         firstPlayer.UpdateBar(SyncScore.scoreKeeper.Player1Score, 27);
        secondPlayer.UpdateBar(SyncScore.scoreKeeper.Player2Score(), 27);

    }
}
