using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//spawns the concord messages at the start of the game
public class ConcordSpawner : MonoBehaviour
{
    [SerializeField] private List<ConcordDialogue> dialogueBlocks = new List<ConcordDialogue>();
    [SerializeField] private GameObject concordPrefab;
    [SerializeField] private GameObject content, canvas;
    [SerializeField] private List<Sprite> userSprites = new List<Sprite>();

    private void Start()
    {
        List<ConcordLine> lines = new List<ConcordLine>();
        foreach (ConcordDialogue block in dialogueBlocks)
        {
            foreach (ConcordLine line in block.dialogue)
                lines.Add(line);
        }


        string prevDate = string.Empty;
        foreach (ConcordLine line in lines)
        {
            GameObject currentMessage = Instantiate(concordPrefab, canvas.transform);
            currentMessage.transform.SetParent(content.transform);

            TextMeshProUGUI textMesh = currentMessage.transform.Find("messagetext").GetComponent<TextMeshProUGUI>();
            if (line.Date != string.Empty)
                currentMessage.transform.Find("date").GetComponent<TextMeshProUGUI>().text = line.Date;
            else
                currentMessage.transform.Find("date").GetComponent<TextMeshProUGUI>().text = prevDate;

            prevDate = System.Text.RegularExpressions.Regex.Unescape(currentMessage.transform.Find("date").GetComponent<TextMeshProUGUI>().text);

            float difference = currentMessage.GetComponent<RectTransform>().sizeDelta.y - textMesh.GetComponent<RectTransform>().sizeDelta.y;
            textMesh.text = line.Text;
            textMesh.ForceMeshUpdate();
            LayoutRebuilder.ForceRebuildLayoutImmediate(textMesh.GetComponent<RectTransform>());
            StartCoroutine(SetBox(textMesh, currentMessage.GetComponent<RectTransform>()));

            currentMessage.transform.Find("name").GetComponent<TextMeshProUGUI>().text = line.User.ToString().ToUpper();

            if (line.User == ConcordUsers.James)
            {
                currentMessage.transform.Find("messagetext").GetComponent<TextMeshProUGUI>().alignment = TMPro.TextAlignmentOptions.MidlineRight;
                continue;
            }
            //currentMessage.transform.Find("profilepic").GetChild(0).GetComponent<Image>().sprite = userSprites[(int)line.User];


        }
    }

    //formats the message
    private IEnumerator SetBox(TextMeshProUGUI text, RectTransform box)
    {
        yield return new WaitForFixedUpdate();
        //Vector2 preferredValues = text.GetPreferredValues(float.PositiveInfinity, float.PositiveInfinity);
        float h = LayoutUtility.GetPreferredHeight(text.GetComponent<RectTransform>());
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(text.GetComponent<RectTransform>().sizeDelta.x, h);
        box.sizeDelta = new Vector2(concordPrefab.GetComponent<RectTransform>().sizeDelta.x, h + 55);
    }
}
