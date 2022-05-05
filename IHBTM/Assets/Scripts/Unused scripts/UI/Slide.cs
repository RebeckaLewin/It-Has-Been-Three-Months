using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField] private float transition;
    [SerializeField] private float hideTime;

    private void Awake()
    {
        startPos = GetComponent<RectTransform>().anchoredPosition;
        startPos.x = 608;
        GetComponent<RectTransform>().anchoredPosition = startPos;
    }

    private void OnEnable()
    {
        GetComponent<RectTransform>().anchoredPosition = startPos;
        LeanTween.moveX(gameObject, 0, transition).setEaseOutCirc();
    }

    public void ShowOptions()
    {
        LeanTween.moveY(GetComponent<RectTransform>(), 253, hideTime).setEaseOutCirc();
    }
    
    public void HideOptions()
    {
        LeanTween.moveY(GetComponent<RectTransform>(), startPos.y, hideTime / 2).setEaseOutCirc();
    }
}
