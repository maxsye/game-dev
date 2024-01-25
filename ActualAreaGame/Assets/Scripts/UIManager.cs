using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

[Serializable()] //marks struct as serializable
public struct UIManagerParameters
{
    [Header("Answers Options")]
    [SerializeField] float margins; //margins between questions
    public float Margins { get { return margins; } }

    [Header("Resolution Screen Options")]
    [SerializeField] Color correctBGColor; //green
    public Color CorrectBGColor { get { return correctBGColor; } }
    [SerializeField] Color incorrectBGColor; //red
    public Color IncorrectBGColor { get { return incorrectBGColor; } }
    [SerializeField] Color finalBGColor; //blue
    public Color FinalBGColor { get { return finalBGColor; } }
}

[Serializable()] //allows us to add objects to this in the inspector
public struct UIElements
{
    //references for the UI
    [SerializeField] RectTransform answersContentArea;
    public RectTransform AnswersContentArea { get { return answersContentArea; } }

    [SerializeField] TextMeshProUGUI questionInfoTextObject; //the question text
    public TextMeshProUGUI QuestionInfoTextObject { get { return questionInfoTextObject; } }

    [SerializeField] TextMeshProUGUI scoreText; //the score text
    public TextMeshProUGUI ScoreText { get { return scoreText; } }

    [SerializeField] TextMeshProUGUI attemptText; //the number of attempts left
    public TextMeshProUGUI AttemptText { get { return attemptText; } }

    [Space]

    [SerializeField] Animator resolutionScreenAnimator; //screen animator
    public Animator ResolutionScreenAnimator { get { return resolutionScreenAnimator; } }

    [SerializeField] Image resolutionBG;
    public Image ResolutionBG { get { return resolutionBG; } }

    [SerializeField] TextMeshProUGUI resolutionStateInfoText; //text with "incorrect" or "correct"
    public TextMeshProUGUI ResolutionStateInfoText { get { return resolutionStateInfoText; } }

    [SerializeField] TextMeshProUGUI resolutionScoreText;
    public TextMeshProUGUI ResolutionScoreText { get { return resolutionScoreText; } }

    [Space]

    [SerializeField] TextMeshProUGUI highScoreText;
    public TextMeshProUGUI HighScoreText { get { return highScoreText; } }

    [SerializeField] CanvasGroup mainCanvasGroup;
    public CanvasGroup MainCanvasGroup { get { return mainCanvasGroup; } }

    [SerializeField] RectTransform finishUIElements; //when the quiz finishes
    public RectTransform FinishUIElements { get { return finishUIElements; } }



}
public class UIManager : MonoBehaviour
{
    public enum ResolutionScreenType { Correct, Incorrect, Finish }

    [Header("References")] //GameManager communicates to UIManager via GameEvents
    [SerializeField] GameEvents events = null;

    [Header("UI Elements (Prefabs)")]
    //whenever it display a new questions, it needs to instantiate a set of answer choices
    [SerializeField] AnswerData answerPreFab = null;

    [SerializeField] UIElements uIElements = new UIElements();

    [Space]

    [SerializeField] UIManagerParameters parameters = new UIManagerParameters();

    List<AnswerData> currentAnswers = new List<AnswerData>();
    //whenever we display a new question and answers,
    //we need to destroy current question and answers

    private int resStateParaHash = 0; //resolutionStateParameterHash

    private IEnumerator IE_DisplayTimedResolution = null;

    void OnEnable()
    {
        events.UpdateQuestionUI += UpdateQuestionUI;
        events.DisplayResolutionScreen += DisplayResolution; //subscribe to delegate
        events.ScoreUpdated += UpdateScoreUI;
    }

    void OnDisable()
    {
        events.UpdateQuestionUI -= UpdateQuestionUI;
        events.DisplayResolutionScreen -= DisplayResolution; //unsubscribe to delegate
        events.ScoreUpdated -= UpdateScoreUI;
    }

    void Start()
    {
        //for the animations of the resolution screen
        UpdateScoreUI();
        resStateParaHash = Animator.StringToHash("ScreenState");
        //ScreenState is the parameter that determines the animation
         //displaying how many attempts left
    }

    void Update () {
        uIElements.AttemptText.text = "Attempts Left: " + DrillController.gemCount.ToString();
    }

    void UpdateQuestionUI(Question question)
    {
        uIElements.QuestionInfoTextObject.text = question.Info; //update the question info
        CreateAnswers(question); //renders the answers
    }

    void DisplayResolution(ResolutionScreenType type, int score) //from GameEvents.cs
    {
        UpdateResUI(type, score);
        uIElements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 2);
        uIElements.MainCanvasGroup.blocksRaycasts = false;
        //displays screen with score

        if (type != ResolutionScreenType.Finish)
        {
            if(IE_DisplayTimedResolution != null)
            {
                StopCoroutine(IE_DisplayTimedResolution);
            }
            IE_DisplayTimedResolution = DisplayTimedResolution();
            StartCoroutine(IE_DisplayTimedResolution); //starts coroutine
        }
        
    }

    IEnumerator DisplayTimedResolution ()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        uIElements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 1);
        uIElements.MainCanvasGroup.blocksRaycasts = true;
    }

    void UpdateResUI(ResolutionScreenType type, int score)
    {
        string gameLevelName = "";
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
        }
        //determine the level name

        var highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
        switch (type)
        { //determines the screens to display
            case ResolutionScreenType.Correct:
                uIElements.ResolutionBG.color = parameters.CorrectBGColor;
                uIElements.ResolutionStateInfoText.text = "Correct!";
                uIElements.ResolutionScoreText.text = "+" + score;
                break;
            case ResolutionScreenType.Incorrect:
                uIElements.ResolutionBG.color = parameters.IncorrectBGColor;
                uIElements.ResolutionStateInfoText.text = "Wrong!";
                uIElements.ResolutionScoreText.text = "-" + score;
                break;
            case ResolutionScreenType.Finish:
                SceneStartScript.gameLevel++; 
                uIElements.ResolutionBG.color = parameters.FinalBGColor;
                uIElements.ResolutionStateInfoText.text = "You have earned the " + gameLevelName + " award!";

                StartCoroutine(CalculateScore());
                uIElements.FinishUIElements.gameObject.SetActive(true);
                uIElements.HighScoreText.gameObject.SetActive(false);
                uIElements.ScoreText.gameObject.SetActive(false);
                uIElements.HighScoreText.text = ((highscore > events.StartupHighscore) ? "New " : string.Empty) + "Highscore: " + highscore;
                //Compare highscore with previous 
                break;

        }
    }
    //A coroutine is a function that has the ability to pause execution and return control
    //to Unity but then to continue where it left off on the following frame.

    IEnumerator CalculateScore() //Calculates the total score
    {
        var scoreValue = 0;
        while (scoreValue < events.CurrentFinalScore) //from GameEvents.cs
        {
            scoreValue++;
            uIElements.ResolutionScoreText.text = scoreValue.ToString();

            yield return null; //will yield for a single frame
        }
    }

    void CreateAnswers(Question question)
    {
        EraseAnswers();
        //after erasing answers, we can create new answers
        float offset = 0 - parameters.Margins;
        for (int i = 0; i < question.Answers.Length; i++)
        {
            AnswerData newAnswer = (AnswerData)Instantiate(answerPreFab, uIElements.AnswersContentArea);
            //created the answers from the answer prefab
            //You can modify the answer prefab to change how to questions are displayed
            newAnswer.UpdateData(question.Answers[i].Info, i);

            newAnswer.Rect.anchoredPosition = new Vector2(0, offset);
            offset -= (newAnswer.Rect.sizeDelta.y + parameters.Margins);
            //positioned

            uIElements.AnswersContentArea.sizeDelta = new Vector2(uIElements.AnswersContentArea.sizeDelta.x, offset * -1);
            //offset is negative, so we multiply by -1
            //you can modify how to answers are positioned

            currentAnswers.Add(newAnswer); //finished creating new answers
        }


    }

    void EraseAnswers()
    {
        foreach (var answer in currentAnswers)
        {
            Destroy(answer.gameObject);
            //erase all the current answers
        }
        currentAnswers.Clear();
        //allow the list to get the new question
    }

    void UpdateScoreUI()
    {
        uIElements.ScoreText.text = "Score: " + events.CurrentFinalScore;
    }//displays the score
}

