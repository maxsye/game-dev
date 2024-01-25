using System;
using System.Collections.Generic;
using UnityEngine;

public enum AnswerType { Single, Multi }

[Serializable()] //allows the inspector to add objects to the answer
public class Answer
{
    public string Info = string.Empty;
    public bool IsCorrect = false;

    public Answer () {} //empty constructor for the XML file
}


[Serializable()]
//Question is scriptable object
public class Question
{
    
    public String Info;
    public Answer[] Answers;
    public Boolean UseTimer = false;
    public Int32 Timer;
    public AnswerType Type = AnswerType.Single;
    public Int32 AddScore;

    public Question () {} //empty constructor for the XML file

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
