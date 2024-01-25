using UnityEngine;

// [System.Serializable]
[CreateAssetMenu(fileName = "GameEvents", menuName = "Quiz/new GameEvents")]
public class GameEvents : ScriptableObject
{
    public delegate void UpdateQuestionUICallback(Question question);
    //updates the new question UI
    public UpdateQuestionUICallback UpdateQuestionUI;

    public delegate void UpdateQuestionAnswerCallback(AnswerData pickedAnswer);
    //updates the question's answer
    public UpdateQuestionAnswerCallback UpdateQuestionAnswer;

    public delegate void DisplayResolutionScreenCallback(UIManager.ResolutionScreenType type, int score);
    //determines which screen to show (correct/wrong), and the score on the screen
    public DisplayResolutionScreenCallback DisplayResolutionScreen;

    public delegate void ScoreUpdatedCallBack(); //updates score
    public ScoreUpdatedCallBack ScoreUpdated;

    [HideInInspector]
    public int CurrentFinalScore = 0;

    [HideInInspector]
    public int StartupHighscore = 0;

}
