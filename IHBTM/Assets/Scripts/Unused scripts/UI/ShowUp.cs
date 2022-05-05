using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowUp : MonoBehaviour
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

    [SerializeField] private Image childImg;
    private Color startColor;
    private Color color;


    private void Start()
    {
        startPos = rt.localPosition;
        startColor = childImg.color;
    }


    private void OnEnable()
    {
        SetValues();
    }

    private void Update()
    {
        if (isStarting)
        {
            Grow();
        }

        if (isClosing)
        {
            if (!childImg.gameObject.activeInHierarchy)
            {
                childImg.gameObject.SetActive(true);
            }
            Close();
        }
    }

    private void SetValues()
    {
        rt = GetComponent<RectTransform>();
        childImg.gameObject.SetActive(true);
        //childImg = transform.GetChild(1).GetComponent<Image>();

        scale = new Vector2(0.3f, 0.3f);
        height = rt.sizeDelta.x;
        position = startPos;

        color = startColor;

        StartCoroutine(Wait());

        isStarting = true;
        isClosing = false;
        StartCoroutine(StopGrow());
    }

    IEnumerator Wait()
    {
        yield return new WaitForFixedUpdate();

        rt.localScale = scale;
        rt.sizeDelta = new Vector2(200, height);
        rt.localPosition = position;

        childImg.color = startColor;

        isStarting = true;
    }

    IEnumerator StopGrow()
    {
        yield return new WaitForSeconds(0.5f);
        isStarting = false;
        childImg.gameObject.SetActive(false);
    }

    void Grow()
    {
        scale = Vector3.Lerp(scale, Vector3.one, Time.deltaTime * speed);
        rt.localScale = scale;

        height = Mathf.LerpUnclamped(height, 300, Time.deltaTime * speed);
        rt.sizeDelta = new Vector2(200, height);

        position = Vector2.Lerp(position, Vector2.zero, Time.deltaTime * speed);
        rt.localPosition = position;
        
        color = Color.Lerp(color, new Color(255, 255, 255, 0), Time.deltaTime * speed * 3);
        childImg.color = color;
    }

    void Close()
    {
        scale = Vector3.Lerp(scale, startScale, Time.deltaTime * speed);
        rt.localScale = scale;

        height = Mathf.LerpUnclamped(height, startHeight, Time.deltaTime * speed);
        rt.sizeDelta = new Vector2(200, height);

        position = Vector2.Lerp(position, startPos, Time.deltaTime * speed);
        rt.localPosition = position;

        color = Color.Lerp(color, startColor, Time.deltaTime * speed);
        childImg.color = color;
    }
}
