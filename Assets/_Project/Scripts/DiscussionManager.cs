using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;
using OpenAI;
using OpenAI.Chat;

public class DiscussionManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] DiscussionBubble bubblePrefab;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Transform bubblesParent;

    [Header(" Events ")]
    public static Action onMessageRecieved;

    [Header(" Authentication")]
    private string apiKey = "";
    private string organizationId = "";
    private OpenAIClient api;

    [Header(" Settings ")]
    [SerializeField] List<ChatPrompt> chatPrompts = new List<ChatPrompt>();


    private void Start()
    {
        CreateBubble("Hey There! How can I help you?", false);
        Authenticate();
        Initialize();
    }

    private void Authenticate()
    {
        api = new OpenAIClient(new OpenAIAuthentication(apiKey, organizationId));
    }

    private void Initialize()
    {
        ChatPrompt prompt = new ChatPrompt("system", "You are a Unity expert.");
        chatPrompts.Add(prompt);
    }

    public async void AskButtonCallback()
    {
        CreateBubble(inputField.text, true);

        ChatPrompt prompt = new ChatPrompt("user", inputField.text);
        chatPrompts.Add(prompt);

        inputField.text = "";

        ChatRequest request = new ChatRequest(
            messages: chatPrompts,
            model: OpenAI.Models.Model.GPT3_5_Turbo,
            temperature: 0.2);// between 0 and 2, higher value dumber, smaller value more deterministic

        try
        {
            var result = await api.ChatEndpoint.GetCompletionAsync(request);

            ChatPrompt chatPrompt = new ChatPrompt("system", result.FirstChoice.ToString());
            chatPrompts.Add(chatPrompt);

            CreateBubble(result.FirstChoice.ToString(), false);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    private void CreateBubble(string message, bool isUserMessage)
    {
        DiscussionBubble discussionBubble = Instantiate(bubblePrefab, bubblesParent);
        discussionBubble.Configure(message, isUserMessage);

        onMessageRecieved?.Invoke();
    }
}
