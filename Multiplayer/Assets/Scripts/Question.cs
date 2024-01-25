using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable()] //allows the inspector to add objects to the answer
public struct Answer
{
    [SerializeField] private string _info; //struct cannot have default values
    public string Info { get { return _info; } }

    [SerializeField] private bool _isCorrect;
    public bool IsCorrect { get { return _isCorrect; } } //getter for _isCorrect
}
[CreateAssetMenu(fileName = "New Question", menuName = "Quiz/new Question")]
//allows Answer to be accessed and created from the inspector

//Question is scriptable object
public class Question : ScriptableObject
{
    public enum AnswerType { Single, Multi }
    [SerializeField] private String _info = string.Empty;
    public String Info { get { return _info; } } //getter for _info

    [SerializeField] Answer[] _answers = null;
    public Answer[] Answers { get { return _answers; } }

    //Parameters

    [SerializeField] private bool _usedTimer = false;
    public bool UseTimer { get { return _usedTimer; } } //getter for _usedTimer

    [SerializeField] private int _timer = 0;
    public int Timer { get { return _timer; } } //getter for _timer

    [SerializeField] private AnswerType _answerType = AnswerType.Multi;
    public AnswerType GetAnswerType { get { return _answerType; } } //getter for _answerType

    [SerializeField] private int _addScore = 10;
    public int AddScore { get { return _addScore; } } //getter for _addScore

    public List<int> GetCorrectAnswers() //returns the correct answers of the questions
    {
        List<int> CorrectAnswers = new List<int>(); //instantiate arrayList
        for (int i = 0; i < Answers.Length; i++)
        {
            if (Answers[i].IsCorrect)
            {
                CorrectAnswers.Add(i); //goes through each question and adds the correct answer of that question
            }
        }
        return CorrectAnswers;

    }

}
