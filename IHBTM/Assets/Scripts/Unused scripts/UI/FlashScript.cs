using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashScript : MonoBehaviour
{
    bool isClosing;
    float speed = 15f;

    private RectTransform rt;

    private Vector3 scale;

    private float startHeight;
    private float height;

    private Vector2 startPos;
    private Vector2 position;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        startPos = rt.localPosition;
        startHeight = rt.rect.height;
    }

    private void OnEnable()
    {
        isClosing = false;

        scale = Vector3.one;
        height = startHeight;
        position = startPos;

        StartCoroutine(Wait());
        StartCoroutine(WaitToClose());
    }

    private void Update()
    {
        if (isClosing)
        {
            Close();
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForFixedUpdate();

        rt.localScale = scale;
        rt.sizeDelta = new Vector2(608, height);
        rt.localPosition = position;
    }

    IEnumerator WaitToClose()
    {
        yield return new WaitForSeconds(0.3f);
        isClosing = true;
        StartCoroutine(DeactivateFlash());
    }

    void Close()
    {
        scale = Vector3.Lerp(scale, Vector3.zero, Time.deltaTime * speed);
        rt.localScale = scale;

        height = Mathf.LerpUnclamped(height, 608, Time.deltaTime * speed);
        rt.sizeDelta = new Vector2(200, height);

        position = Vector2.Lerp(position, Vector3.zero, Time.deltaTime * speed);
        rt.localPosition = position;
    }

    IEnumerator DeactivateFlash()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
