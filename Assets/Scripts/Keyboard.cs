using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Keyboard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool IsPressed { get; private set; }
    Image m_Image;
    Color m_DefaultColor;


    void Start()
    {
        m_Image = GetComponent<Image>();
        m_DefaultColor = m_Image.color;
    }

    public void OnPointerDown(PointerEventData data)
    {
        IsPressed = true;
        m_Image.color = m_DefaultColor * 0.8f;
        Debug.Log(m_DefaultColor);
    }

    public void OnPointerUp(PointerEventData data)
    {
        IsPressed = false;
        m_Image.color = m_DefaultColor;
    }
}
