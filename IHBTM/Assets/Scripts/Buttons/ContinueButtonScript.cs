using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//formats the continue button
public class ContinueButtonScript : MonoBehaviour
{
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
        SetButtonText();
    }

    void SetButtonText()
    {
        if(dm.lineNum < dm.currentBlock.lines.Count)
        {
            text.SetText(dm.dialogBlocks[dm.blockNum].lines[dm.lineNum].line);
        }
        else { text.SetText(dm.dialogBlocks[dm.blockNum + 1].lines[0].line); }

        if(text.preferredWidth > rt.sizeDelta.x)
        {
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, 80);
        }
        else { rt.sizeDelta = new Vector2(rt.sizeDelta.x, 40); }
    }
}
