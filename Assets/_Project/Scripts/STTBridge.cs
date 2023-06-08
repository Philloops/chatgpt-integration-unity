using Meta.WitAi.Json;
using Meta.WitAi;
using Oculus.Voice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class STTBridge : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TMP_InputField promptInputField;

    [Header("Voice")]
    [SerializeField] AppVoiceExperience appVoiceExperience;

    // Whether voice is activated
    public bool IsActive => _active;
    private bool _active = false;

    // Add delegates
    private void OnEnable()
    {
        appVoiceExperience.events.OnRequestCreated.AddListener(OnRequestStarted);
        appVoiceExperience.events.OnPartialTranscription.AddListener(OnRequestTranscript);
        appVoiceExperience.events.OnFullTranscription.AddListener(OnRequestTranscript);
        appVoiceExperience.events.OnStartListening.AddListener(OnListenStart);
        appVoiceExperience.events.OnStoppedListening.AddListener(OnListenStop);
        appVoiceExperience.events.OnStoppedListeningDueToDeactivation.AddListener(OnListenForcedStop);
        appVoiceExperience.events.OnStoppedListeningDueToInactivity.AddListener(OnListenForcedStop);
        appVoiceExperience.events.OnResponse.AddListener(OnRequestResponse);
        appVoiceExperience.events.OnError.AddListener(OnRequestError);
    }
    // Remove delegates
    private void OnDisable()
    {
        appVoiceExperience.events.OnRequestCreated.RemoveListener(OnRequestStarted);
        appVoiceExperience.events.OnPartialTranscription.RemoveListener(OnRequestTranscript);
        appVoiceExperience.events.OnFullTranscription.RemoveListener(OnRequestTranscript);
        appVoiceExperience.events.OnStartListening.RemoveListener(OnListenStart);
        appVoiceExperience.events.OnStoppedListening.RemoveListener(OnListenStop);
        appVoiceExperience.events.OnStoppedListeningDueToDeactivation.RemoveListener(OnListenForcedStop);
        appVoiceExperience.events.OnStoppedListeningDueToInactivity.RemoveListener(OnListenForcedStop);
        appVoiceExperience.events.OnResponse.RemoveListener(OnRequestResponse);
        appVoiceExperience.events.OnError.RemoveListener(OnRequestError);
    }

    // Request began
    private void OnRequestStarted(WitRequest r)
    {
        // Begin
        _active = true;
    }
    // Request transcript
    private void OnRequestTranscript(string transcript)
    {
        promptInputField.text = transcript;
    }
    // Listen start
    private void OnListenStart()
    {
        //promptInputField.text = "Listening...";
    }
    // Listen stop
    private void OnListenStop()
    {
        //promptInputField.text = "Processing...";
    }
    // Listen stop
    private void OnListenForcedStop()
    {
        OnRequestComplete();
    }
    // Request response
    private void OnRequestResponse(WitResponseNode response)
    {
        if (!string.IsNullOrEmpty(response["text"]))
        {
            promptInputField.text = response["text"];
        }
        OnRequestComplete();
    }
    // Request error
    private void OnRequestError(string error, string message)
    {
        promptInputField.text = $"<color=\"red\">Error: {error}\n\n{message}</color>";
        OnRequestComplete();
    }
    // Deactivate
    private void OnRequestComplete()
    {
        _active = false;
    }

    // Toggle activation
    public void ToggleActivation()
    {
        SetActivation(!_active);
    }
    // Set activation
    public void SetActivation(bool toActivated)
    {
        if (_active != toActivated)
        {
            _active = toActivated;
            if (_active)
            {
                appVoiceExperience.Activate();
            }
            else
            {
                appVoiceExperience.Deactivate();
            }
        }
    }
}
