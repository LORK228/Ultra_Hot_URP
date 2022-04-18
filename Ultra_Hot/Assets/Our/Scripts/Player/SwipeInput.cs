using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SwipeInput : MonoBehaviour, IDragHandler 
{
    [SerializeField] public ControllerAndroid android;

    public void OnDrag(PointerEventData eventData)
    {
        android.mouseX = eventData.delta.x;
        android.mouseY = eventData.delta.y;
    }
}
