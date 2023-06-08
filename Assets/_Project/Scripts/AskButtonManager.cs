using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class AskButtonManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] DiscussionManager discussionManager;
    [SerializeField] STTBridge sttBridge;
    [SerializeField] TMP_InputField promptInputField;

    [Header(" Graphics ")]
    [SerializeField] GameObject askText;
    [SerializeField] GameObject micImage;

    [Header(" Settings ")]
    private bool recording;

    private void Start()
    {
        ShowMicImage();
        promptInputField.onValueChanged.AddListener(InputFieldValueChangedCallback);
    }

    public void PointerDownCallback()
    {
        if(promptInputField.text.Length > 0)
        {
            discussionManager.AskButtonCallback();
        }
        else
        {
            sttBridge.SetActivation(true);
            recording = true;
        }
    }

    public void PointerUpCallback() 
    {
        sttBridge.SetActivation(false);
        recording = false;

        InputFieldValueChangedCallback(promptInputField.text);
    }
    private void InputFieldValueChangedCallback(string prompt)
    {
        if (recording)
            return;

        if(prompt.Length <= 0)
        {
            ShowMicImage();
        }
        else
        {
            ShowAskText();
        }
    }
    private void ShowMicImage()
    {
        askText.SetActive(false);
        micImage.SetActive(true);
    }
    private void ShowAskText()
    {
        micImage.SetActive(false);
        askText.SetActive(true);
    }
}
