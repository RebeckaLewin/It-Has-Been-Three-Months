using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUp2 : MonoBehaviour
{
    private bool isStarting;
    public bool isClosing;
    private float speed = 15f;

    private RectTransform rt;

    private Vector3 startScale;
    private Vector3 scale;

    private float startHeight;
    private float height;

    private Vector2 startPos;
    private Vector2 position;


    private void Start()
    {

    }


    private void OnEnable()
    {
        rt = GetComponent<RectTransform>();

        SetValues();

        startPos = rt.anchoredPosition;
        startScale = rt.localScale;
        startHeight = rt.sizeDelta.y;
    }

    private void Update()
    {
        if (isStarting)
        {
            Transition(Vector3.one, 300, Vector3.zero);
        }

        if (isClosing)
        {
            Transition(Vector3.zero, startHeight, startPos);
        }
    }

    private void SetValues()
    {
        scale = new Vector2(0.3f, 0.3f);
        height = 200;
        position = startPos;

        StartCoroutine(Wait());

        isStarting = true;
        isClosing = false;
        StartCoroutine(StopOpen());
    }

    IEnumerator Wait()
    {
        yield return new WaitForFixedUpdate();

        rt.localScale = scale;
        rt.sizeDelta = new Vector2(200, height);
        rt.anchoredPosition = position;

        isStarting = true;
    }

    IEnumerator StopOpen()
    {
        yield return new WaitForSeconds(0.5f);
        isStarting = false;
    }

    void Transition(Vector3 targetScale, float targetHeight, Vector3 targetPos)
    {
        scale = Vector3.Lerp(scale, targetScale, Time.deltaTime * speed);
        rt.localScale = scale;

        height = Mathf.LerpUnclamped(height, targetHeight, Time.deltaTime * speed);
        rt.sizeDelta = new Vector2(200, height);

        position = Vector2.Lerp(position, targetPos, Time.deltaTime * speed);
        rt.anchoredPosition = position;
    }
}
