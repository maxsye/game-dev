using UnityEngine;
using UnityEngine.UI;
using TMPro; //Text Mesh Pro

public class AnswerData : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI infoTextObject;
    [SerializeField] Image toggle;

    [Header("Textures")]
    [SerializeField] Sprite uncheckedToggle;
    [SerializeField] Sprite checkedToggle;

    [Header("References")]
    [SerializeField] GameEvents events;

    private RectTransform _rect;
    public RectTransform Rect {
        get {
            if(_rect == null)
             {
                 _rect = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
                //we will try to get the component of type rect transform, but if we can't
                //we will add component of type rect transform
             }
             return _rect;
        }
    }

    private int _answerIndex = -1;
    public int AnswerIndex { get { return _answerIndex; } }

    private bool Checked = false;

    public void UpdateData(string info, int index) //updates the question data
    {
        infoTextObject.text = info;
        _answerIndex = index;
    }

    public void Reset()
    {
        Checked = false;
        UpdateUI();
    }

    public void SwitchState()
    {
        Checked = !Checked;
        //reverses Checked
        UpdateUI();

        if (events.UpdateQuestionAnswer != null)
        {
            events.UpdateQuestionAnswer(this); //sends class as parameter
        }
    }

    void UpdateUI()
    {
        if (toggle == null) return;
        toggle.sprite = (Checked) ? checkedToggle : uncheckedToggle;
        //if Checked is true toggle.sprite is set equal to checkedToggle
        //toggle.sprite is set equal to uncheckedToggle otherwise
    }
}
