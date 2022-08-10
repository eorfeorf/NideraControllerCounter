using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SettingView : MonoBehaviour
{
    [SerializeField]
    private Button openButton;
    [SerializeField]
    private Button closeButton;

    public IObservable<Unit> OnOpen() => openButton.OnClickAsObservable();
    public IObservable<Unit> OnClose() => closeButton.OnClickAsObservable();

    public void SetActiveOpenButton(bool isActive)
    {
        openButton.gameObject.SetActive(isActive);
    }
    
    public void SetActiveCloseButton(bool isActive)
    {
        closeButton.gameObject.SetActive(isActive);
    }
}
