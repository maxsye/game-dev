using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static Question[] _questions = null; //list of questions
    public int questionsCorrect; //how many questions they have got correct
    public static int questionAttempts;

    private int saveMe = 100;
 
    public Question[] Questions { get { return _questions; } } //getter for _questions

    [SerializeField] GameEvents events = null; 

    [SerializeField] Animator timerAnimator = null;
    [SerializeField] TextMeshProUGUI timerText = null;
    [SerializeField] Color timerHalfWayOutColor = Color.yellow; //timer colors
    [SerializeField] Color timerAlmostOutColor = Color.red; //you can change the colors to your liking
    private Color timerDefaultColor = Color.white;

    public static bool gameStart = true;

    private List<AnswerData> PickedAnswers = new List<AnswerData>();

    private List<int> FinishedQuestions = new List<int>();
    private int currentQuestion = 0;
    private int timerStateParaHash = 0;

    private IEnumerator IE_WaitTillNextRound = null; //IEnumerator is an iteration over a collection
    private IEnumerator IE_StartTimer = null;

    private bool IsFinished //determines whether the quiz is finished
    {
        get
        {
            return (FinishedQuestions.Count < Questions.Length) ? false : true;
        }
    }

    void OnEnable()
    {
        events.UpdateQuestionAnswer += UpdateAnswers; //subscribe
    }

    void OnDisable()
    {
        events.UpdateQuestionAnswer -= UpdateAnswers; //unsubscribe
    }

    void Awake() //called before start, curent score is 0
    {
        events.CurrentFinalScore = 0;
    }

    void Start() //when the game starts
    {
        events.StartupHighscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey); //get the highscore from GameUtility

        timerDefaultColor = timerText.color;

        if (gameStart) {
            LoadQuestions(); //load the first questions
            gameStart = false;
        }

        timerStateParaHash = Animator.StringToHash("TimerState");

        var seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue); //makes answers random
        UnityEngine.Random.InitState(seed);

        Display(); //call the display function
    }

    public void UpdateAnswers(AnswerData newAnswer)
    { //updated the picked answers, when the user chooses their answer
        if (Questions[currentQuestion].GetAnswerType == Question.AnswerType.Single)
        { //if there is one correct answer
            foreach (var answer in PickedAnswers) //check for already picked answer
            {
                if (answer != newAnswer)
                {
                    answer.Reset();
                }

            }
            PickedAnswers.Clear();
            PickedAnswers.Add(newAnswer);
        }
        else //if there are multiple correct answers
        {
            bool alreadyPicked = PickedAnswers.Exists(x => x == newAnswer); //check if answer already exists in PickedAnswers list
            if (alreadyPicked)
            {
                PickedAnswers.Remove(newAnswer);
            }
            else
            { //if not picked, add new answer
                PickedAnswers.Add(newAnswer);
            }
        }
    }

    public void EraseAnswers()
    {
        PickedAnswers = new List<AnswerData>();
    }
    public bool allCorrect()
    {

        for (int i = 0; i < Questions.Length; i++)
        {
            if (Questions[i] != null)
            {
                return false; 
            }
        
        }
        return true;
    }
    void Display()
    {
        saveMe = 100;
        if (DrillController.gemCount <= 0)
        {
            SceneManager.LoadScene("GameScene");
        }
        
        EraseAnswers();
        var question = GetQuestion();

        if (events.UpdateQuestionUI != null)
        {
            events.UpdateQuestionUI(question); //displays current question information
        }
        else
        {
            Debug.LogWarning("Something wrong in gameManager.cs, void Display()");
        }

        if (question.UseTimer) //if question uses timer, display it
        {
            UpdateTimer(question.UseTimer);
        }
    }

    public void Accept() //accept the answer and move on the the next questoin
    {
        DrillController.gemCount -= 1;
        questionAttempts++;
        UpdateTimer(false);
        bool isCorrect = CheckAnswers();
        Debug.Log(isCorrect);
        
        //if not correct, keep the question in the potential questions list
        //you can change it by deleting the if statement, so that regardless of the condition, the question moves on

        Debug.Log(currentQuestion);
        Debug.Log(Questions.Length);    
        UpdateScore((isCorrect) ? Questions[currentQuestion].AddScore : -Questions[currentQuestion].AddScore);
        //either adds or subtracts the score

        if (IsFinished)
        {
            SetHighScore();
        }
        //sets highscore after updating the highscore

        var type = (IsFinished)
        ? UIManager.ResolutionScreenType.Finish
        : (isCorrect) ? UIManager.ResolutionScreenType.Correct
        : UIManager.ResolutionScreenType.Incorrect;
        //Whether to load finish screen, correct screen, or incorrect screen

        if (events.DisplayResolutionScreen != null)
        {
            events.DisplayResolutionScreen(type, Questions[currentQuestion].AddScore);
        }
        if (isCorrect)
        {
            _questions[currentQuestion] = null;
            //e FinishedQuestions.Add(currentQuestion); //if correct, then move to next question
            questionsCorrect++;
        }
        currentQuestion ++;
        currentQuestion = currentQuestion % Questions.Length;
        AudioManager.Instance.PlaySound((isCorrect) ? "CorrectSFX" : "IncorrectSFX");
        
        if (type != UIManager.ResolutionScreenType.Finish)
        {
            if (IE_WaitTillNextRound != null)
            {
                StopCoroutine(IE_WaitTillNextRound); //stop the current coroutine
            }


            IE_WaitTillNextRound = WaitTillNextRound();
            StartCoroutine(IE_WaitTillNextRound);
        }
        
        

    }

    void UpdateTimer(bool state)
    {
        switch (state)
        {
            case true: //show the timer
                IE_StartTimer = StartTimer();
                StartCoroutine(IE_StartTimer);

                timerAnimator.SetInteger(timerStateParaHash, 2);
                break;
            case false:  //hide the timer
                if (IE_StartTimer != null)
                {
                    StopCoroutine(IE_StartTimer);
                }

                timerAnimator.SetInteger(timerStateParaHash, 1);
                break;

        }
    }

    IEnumerator StartTimer() //starts the timer with the relevant information
    {
        var totalTime = Questions[currentQuestion].Timer;
        var timeLeft = totalTime;

        timerText.color = timerDefaultColor;


         while (timeLeft > 0 && saveMe >= 0)
        {
            saveMe -= 1;
            Debug.Log ("its the first loop");
            if (Input.GetKey (KeyCode.B)) {
                break;
            }
            timeLeft--;

            AudioManager.Instance.PlaySound("CountdownSFX"); //play sound effect

            if (timeLeft < totalTime / 2 && timeLeft > totalTime / 4) //halfway done with question
            {
                timerText.color = timerHalfWayOutColor;
            }
            if (timeLeft < totalTime / 4) //timer is almost done
            {
                timerText.color = timerAlmostOutColor;
            }
            timerText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        Accept();

    }

    IEnumerator WaitTillNextRound()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime); //wait for certain amount of time
        Display();
    }

    Question GetQuestion()
    {
        //var index = GetQuestionIndex(); //stores index
        Debug.Log (allCorrect());
        if (allCorrect()) {
            SceneManager.LoadScene("GameScene");
            gameStart = true;
            SceneStartScript.gameLevel  += 1;
            return null;
        }
        int index = 0; 
        while (Questions[index] == null)
        {
            if (saveMe <= 0) {
                Debug.Log ("used saveMe oop");
                break;
            }
            saveMe -= 1;
            Debug.Log ("its the second loop");
            index += 1;      
        }
        currentQuestion = index; 
        return Questions[currentQuestion];
    }
    int GetQuestionIndex()
    {
        int questionIndex = -1;
        if (FinishedQuestions.Count < Questions.Length)  //have some questions left
        {
            do
            {
                questionIndex++;
            } while (FinishedQuestions.Contains(questionIndex) || questionIndex == currentQuestion); //does it contain index
        }
        return questionIndex;
    }

    bool CheckAnswers()
    {
        if (CompareAnswers())
        {
            return true;
            
        }
        return false;
    }

    bool CompareAnswers()
    {
        if (PickedAnswers.Count > 0) //check if have selected an answer
        {
            List<int> c = Questions[currentQuestion].GetCorrectAnswers(); //corect answers
            List<int> p = PickedAnswers.Select(x => x.AnswerIndex).ToList(); //picked answers

            //compares the two lists
            var f = c.Except(p).ToList();
            //.Except comes with System.linq, removes everything from list except element x
            var s = p.Except(c).ToList();

            //check if list contains elements
            return !f.Any() && !s.Any();
        }
        return false;
    }
    void LoadQuestions() //loads a random question set
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
        //get the game level name

        Object[] objs = Resources.LoadAll(gameLevelName, typeof(Question));
        _questions = new Question[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            _questions[i] = (Question)objs[i];
        }
        //iterates through all the questions
    }

    public void RestartGame()
    {
        LoadQuestions();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void SetHighScore()
    {
        //check current high score and previous high score
        var highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
        if (highscore < events.CurrentFinalScore)
        {
            //beat the highscore
            PlayerPrefs.SetInt(GameUtility.SavePrefKey, events.CurrentFinalScore);
        }

    }

    private void UpdateScore(int add)
    {
        events.CurrentFinalScore += add;

        if (events.ScoreUpdated != null)
        {
            events.ScoreUpdated(); //call the delegate if not null
        }
    }
}
