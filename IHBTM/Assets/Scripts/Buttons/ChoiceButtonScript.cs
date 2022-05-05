using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Attaches to the choice button, sets it text and formats the UI
public class ChoiceButtonScript : MonoBehaviour
{  
    [SerializeField] bool isFirstOption;
    TextMeshProUGUI text;
    public DialogueManager dm;
    private RectTransform rt;

    private void Start()
    {
        dm = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        rt = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        StartCoroutine(WaitTilLast());
    }

    IEnumerator WaitTilLast()
    {
        yield return new WaitForFixedUpdate();
        if (isFirstOption)
        {
            SetButtonText(1);
        }
        else { SetButtonText(2); }
    }

    void SetButtonText(int i)
    {
        text.SetText(dm.dialogBlocks[dm.blockNum + i].lines[0].line);

        if (text.preferredWidth > rt.sizeDelta.x)
        {
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, 80);
        }
        else { rt.sizeDelta = new Vector2(rt.sizeDelta.x, 40); }
    }
}
