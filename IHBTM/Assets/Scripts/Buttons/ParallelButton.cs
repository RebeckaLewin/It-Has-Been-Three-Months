using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//formats the parallelbutton (used for the branch one as well)
public class ParallelButton : MonoBehaviour
{
    private TextMeshProUGUI text;
    private RectTransform rt;

    private void Awake()
    {
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        rt = GetComponent<RectTransform>();
    }

    public void SetText(string t)
    {
        text.text = t;

        if (text.preferredWidth > rt.sizeDelta.x)
        {
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, 80);
        }
        else { rt.sizeDelta = new Vector2(rt.sizeDelta.x, 40); }
    }
}
