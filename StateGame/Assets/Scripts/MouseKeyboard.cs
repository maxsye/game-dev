using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//determines whether a button is already selected between the mouse and keybaord interfaces
//to prevent selecting two buttons at a time
//allows keyboard and mouse interface to communicate with each other

//requires the GameObject to be a Selectable, like a button
[RequireComponent(typeof(Selectable))]
public class MouseKeyboard : MonoBehaviour, IPointerEnterHandler, IDeselectHandler 
//implements the MonoBehaviour, IPointerEnterHandler, IDeselectHandler interfaces
{
    public void OnPointerEnter(PointerEventData eventData)
    //takes a pointer event as a parameter, event can be from mouse or keyboard
    {
        if (!EventSystem.current.alreadySelecting) //if there are no other items selected
            EventSystem.current.SetSelectedGameObject(this.gameObject); //then select this gameObject
    }
    public void OnDeselect(BaseEventData eventData)
    //the OnDeselect function takes a BaseEventData as a parameter
    {
        this.GetComponent<Selectable>().OnPointerExit(null);
        //if this GameObject is selectable, when the pointer exits, deselect this object
    }
}
