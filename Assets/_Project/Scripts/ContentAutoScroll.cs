using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentAutoScroll : MonoBehaviour
{
    private RectTransform rt;
    private float scrollDelay = 0.3f;

    private void Start()
    {
        rt = GetComponent<RectTransform>();

        DiscussionManager.onMessageRecieved += DelayScrollDown;
    }

    private void OnDestroy()
    {
        DiscussionManager.onMessageRecieved -= DelayScrollDown;
    }

    private void DelayScrollDown()
    {
        Invoke("ScrollDown", scrollDelay);
    }

    private void ScrollDown()
    {
        Vector2 anchoredPosition = rt.anchoredPosition;
        anchoredPosition.y = Mathf.Max(0, rt.sizeDelta.y);
        rt.anchoredPosition = anchoredPosition;
    }
}
