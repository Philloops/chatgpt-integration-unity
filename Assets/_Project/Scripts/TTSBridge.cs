using Meta.WitAi.TTS;
using Meta.WitAi.TTS.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTSBridge : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] TTSSpeaker speaker;

    private void Start()
    {
        DiscussionBubble.onVoiceButtonClicked += VoiceButtonClickedCallback;
    }

    private void OnDestroy()
    {
        DiscussionBubble.onVoiceButtonClicked -= VoiceButtonClickedCallback;
    }

    private void VoiceButtonClickedCallback(string message)
    {
        if(speaker.IsSpeaking)
        {
            Debug.Log("Stopping the speaker");
            speaker.Stop();
        }
        else
        {
            Debug.Log("Started speaking");
            Speak(message);
        }
    }

    private void Speak(string message)
    {
        // SPLIT CHATGPT ANSWER
        string[] messages = message.Split('.');
        speaker.StartCoroutine(speaker.SpeakQueuedAsync(messages));

    }
}
