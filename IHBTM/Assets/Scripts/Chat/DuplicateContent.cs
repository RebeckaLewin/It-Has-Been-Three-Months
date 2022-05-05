using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//follows after the original content object, for a smooth scrolling experience
public class DuplicateContent : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private float speed;

    RectTransform rt, contentRT;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        contentRT = content.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowOriginal();
        UpdateSize();
    }

    void FollowOriginal()
    {
        float targetY = contentRT.anchoredPosition.y;
        Vector3 targetPos = new Vector3(0, targetY, 0);
        rt.anchoredPosition = Vector3.Lerp(rt.anchoredPosition, targetPos, Time.deltaTime * speed);

    }

    void UpdateSize()
    {
        Vector2 targetSize = new Vector2(0, contentRT.sizeDelta.y);
        rt.sizeDelta = Vector2.Lerp(rt.sizeDelta, targetSize, Time.deltaTime * speed);
    }
}
