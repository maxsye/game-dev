using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SyncScore : NetworkBehaviour
{
    
    [SyncVar]
    public int player1Score = 0;
    public int Player1Score { get { return player2Score; } }
    
    public static SyncScore scoreKeeper;
    public static bool server; 

    //public SyncScore scoring;

    [SyncVar]
    public int player2Score = 0;
   // public int Player2Score { get { return player2Score; } }
    public int Player2Score(){
        return player2Score; 
    }
    public override void OnStartServer()
    { 

Debug.Log("BEFOREEEEEEEEE");
        base.OnStartServer();
        server = isServer;
        
        //scoring = new SyncScore();
    }

    // public override void OnStartClient()
    // {
    //     base.OnStartClient();
    // }

    public static void changeScore (SyncScore scorekeeper2, int score) {
        //scorekeeper2.GetComponent<SyncScore>().CmdScoreChange(score);
        Debug.Log("SCOREKEEPER2 is " + scorekeeper2.player1Score);
        scorekeeper2.CmdScoreChange(score);
       
    }

    //[Command]
    public void CmdScoreChange(int score)
    {
        if (server) {
            Debug.Log("SERVER "+server + " " + score);
            player1Score = score;
        } else {
            player2Score = score;
            Debug.Log("CLIENT"+server + " " + score);
        }
    }
   

    // [Command]
    // private void Player2ScoreChange(int score)
    // {
    //     player2Score = score;
    // }

    // private IEnumerator _updateScores()
    // {
    //     WaitForSeconds wait = new WaitForSeconds(1f);

    //     while (true)
    //     {
    //         yield return wait;
            
    //     }
    // }
    // }
    // }


    // // Update is called once per frame
    // public override void OnStartServer()
    // {
    //     base.OnStartServer();
    //     StartCoroutine(_RandomizeColor());
    // }
    // private void SetColor(Color32 color)
    // {
    //     SpriteRenderer renderer = GetComponent<SpriteRenderer>();
    //     renderer.color = color;
    // }
    // private IEnumerator _RandomizeColor()
    // {
    //     WaitForSeconds wait = new WaitForSeconds(2f);

    //     while (true)
    //     {
    //         yield return wait;

    //         _color = Random.ColorHSV(0f, 1f, 1f, 1f, 0f, 1f, 1f, 1f);
    //     }
    // }
}
