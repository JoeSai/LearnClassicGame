using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour , IPointerEnterHandler
{
    private Button _button;

    private void Awake()
    {
        _button = transform.GetComponent<Button>();
        _button.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlaySound("ButtonClick");
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MusicMgr.GetInstance().PlaySound("ButtonOver");
    }
    
}
