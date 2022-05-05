using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideTransition : Transition
{
    private Vector3 startPos;
    [SerializeField] private float time;
    bool isActive;

    private void Awake()
    {
        startPos = GetComponent<RectTransform>().anchoredPosition;
        startPos.x = 608;
        GetComponent<RectTransform>().anchoredPosition = startPos;
    }

    private void Start()
    {
        isActive = false;
    }

    private void OnEnable()
    {
        if(!isActive)
            Enter();
    }

    public override void Enter()
    {
        GetComponent<RectTransform>().anchoredPosition = startPos;
        LeanTween.moveX(gameObject, 0, time).setEaseOutCirc();
        isActive = true;
    }

    public override void Exit()
    {
        LeanTween.moveX(GetComponent<RectTransform>(), 608, time).setEaseOutCirc();
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
        isActive = false;
    }
}
