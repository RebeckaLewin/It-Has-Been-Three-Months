using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//animation for the chatbuttons
public class ChatbtnTransition : MonoBehaviour
{
    [SerializeField] private float transitionTime;
    private Vector2 startSize;

    private void Start()
    {
        startSize = GetComponent<RectTransform>().sizeDelta;
    }

    public void Format()
    {
        LeanTween.size(GetComponent<RectTransform>(), new Vector2(530, startSize.y), transitionTime);
    }

    public void Revert()
    {
        LeanTween.size(GetComponent<RectTransform>(), startSize, transitionTime);
    }
}
