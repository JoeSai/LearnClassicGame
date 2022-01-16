using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForStartWindow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private VoidEventChannelSO _startGameEvent = default;
    private void OnEnable()
    {
        _startGameEvent.OnEventRaised += Hide;
    }

    private void OnDisable()
    {
        _startGameEvent.OnEventRaised -= Hide;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    
}
