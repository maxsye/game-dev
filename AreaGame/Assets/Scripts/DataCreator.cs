using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; //works with editor scripts

public class DataCreator_Window : EditorWindow //creates a custom window in the unity editor
{
    public Data data = new Data(); //creates a Data object from the Data class
    private Vector2 scrollPosition = Vector2.zero; //the user's scroll position
    SerializedObject serializedObject = null;
    //these Serialized classes are used for editing serialized fields on unity objects
    SerializedProperty questionsProperty = null;
    private void OnEnable() //called whenever this behavior is enabled
    {
        serializedObject = new SerializedObject(this);
        //we want this variable to represent the object we are currently inspecting
        data.Questions = new Question[0]; //empty list
        questionsProperty = serializedObject.FindProperty("data").FindPropertyRelative("Questions");
        //find the data property and access the data's question property
    }
    [MenuItem("Game/Data Creator")] //creates a new menu item, accessed by top toolbar
    public static void OpenFileWindow() //displays the window with minimum size
    {
        var window = EditorWindow.GetWindow<DataCreator_Window>("Creator");
        //get the window, the data creater window
        //creates a window in the unity inspector
        window.minSize = new Vector2(510f, 344f); //the minimum size in the inspector
        window.Show(); //shows the editor window in the inspector
    }

    private void OnGUI()
    //everything we want on the editor window must be put in this method, basically creates the user interface on the window
    {
        #region Header

        Rect headerRect = new Rect(15, 15, this.position.width - 30, 65); //creates a rectangle
        GUI.Box(headerRect, GUIContent.none);
        //creates a box with position of headerRect with no content

        GUIStyle headerStyle = new GUIStyle(EditorStyles.largeLabel);
        //creating a new GUI style that is indentical to largeLabel style
        headerStyle.fontSize = 26; //font size of the header text
        headerStyle.alignment = TextAnchor.UpperLeft; //alignment of the header text

        headerRect.x += 5; //adds 5 to x position of rectangle
        headerRect.width -= 10; //decreases width of rectangle by 10
        headerRect.y += 5; //increases y position of rectangle by 5
        headerRect.height -= 10; //decreases height of rectangle by 10
        //we do this so when we use the headerRect variable, the content will be
        //based on this new rectangle, so there will be some margin between original rectangle and the content

        GUI.Label(headerRect, "Data to XML Creator", headerStyle);
        //creates text in the headerRect rectangle with a text syle of headerStyle

        Rect summaryRect = new Rect(headerRect.x + 25, (headerRect.y + headerRect.height) - 20, headerRect.width - 50, 15);
        //creates a new rectangle within the headerRect rectangle
        GUI.Label(summaryRect, "Create the data that needs to be included into the XML file");
        //creates text in the summaryRect rectangle with the default text style
        #endregion
        
        #region Body
        Rect bodyRect = new Rect(15, (headerRect.y + headerRect.height) + 20, this.position.width - 30,
        this.position.height - (headerRect.y + headerRect.height) - 80);
        //creates another rectangle taking into consideration the headerRect, doesn't overlap headerRect 
        GUI.Box(bodyRect, GUIContent.none); //creates a box with position of bodyRect with no content

        var arraySize = data.Questions.Length;

        Rect viewRect = new Rect(bodyRect.x + 10, bodyRect.y + 10, bodyRect.width - 20, EditorGUI.GetPropertyHeight(questionsProperty));
        //creates a rectangle inside bodyRect, with a height of the property height of questionsProperty
        Rect scrollPositionRect = new Rect(viewRect);
        //creates a rectangle indentical to viewRect
        scrollPositionRect.height = bodyRect.height - 20;
        //sets scrollPositionRect's height

        scrollPosition = GUI.BeginScrollView(scrollPositionRect, scrollPosition, viewRect, false, false,
        GUIStyle.none, GUI.skin.verticalScrollbar);
        //creates a scrolling view with scrollPositionRect as position, viewRect as the viewer's view,
        //doesn't always show horizontal or vertical, and the vertical scroll has a style of
        //GUI.skin.verticalScrollbar

        var drawSlider = viewRect.height > scrollPositionRect.height;
        //boolean of whether the viewRect's height is greater than scrollPositionRect's height

        Rect propertyRect = new Rect(bodyRect.x + 10, bodyRect.y + 10, bodyRect.width - (drawSlider ? 40 : 20), 17);
        //creates a rectangle inside bodyRect
        //if drawSlider is true, minus 40, otherwise minus 20
        EditorGUI.PropertyField(propertyRect, questionsProperty, true);
        //this creates a questionsProperty in the propertyRect rectangle
        //basically creates a dropdown menu when clicked
        //the true parameter means that this dropdown has children

        serializedObject.ApplyModifiedProperties(); //apply all the modified properties

        GUI.EndScrollView(); //ending the scroll biew
        #endregion

        #region Navigation

        Rect buttonRect = new Rect(bodyRect.x + bodyRect.width - 85, bodyRect.y + bodyRect.height + 15,
        85, 30); //creates a rectangle based on bodyRect
        bool pressed = GUI.Button(buttonRect, "Create", EditorStyles.miniButtonRight);
        //creates a button with the buttonRect rectangle, with text "Create", textstyle of miniButtonRight
        if (pressed) //if the button is pressed
        {
            Data.Write(data); //write the data
        }
        buttonRect.x -= buttonRect.width;  //subtract buttonRect's width from buttonRect's x position
        pressed = GUI.Button(buttonRect, "Fetch", EditorStyles.miniButtonLeft);
        //creates another button with the buttonRect rectangle, with text "Fetch, textstyle of miniButtonLeft
        if (pressed) //if the button is pressed
        {
            var d = Data.Fetch(out bool result); //fetch the data and store it in local variable
            if (result) //if result is true
            {
                data = d; //set data equal to the local variable d
            }
            serializedObject.ApplyModifiedProperties(); //apply the properties to the serializedObject
            serializedObject.Update(); //updates the serializedObject
        }
        #endregion
    }

}
