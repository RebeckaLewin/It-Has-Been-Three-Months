using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTransition : Transition
{
    [SerializeField] private float time;

    private void OnEnable()
    {
        Enter();
    }

    public override void Enter()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 1, time);
    }

    public override void Exit()
    {
        LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 0, time);
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

}
