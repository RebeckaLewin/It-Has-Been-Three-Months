using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//fills the chat with static messages
public class ChatFiller : MonoBehaviour
{
    [SerializeField] private DialogueBlock block;
    [SerializeField] private GameObject content, cloneContent;
    [SerializeField] private GameObject message, cloneMessage;
    private Canvas canvas;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        foreach (SingleLine l in block.lines)
        {
            GameObject original = Instantiate(message, canvas.transform);
            GameObject clone = Instantiate(cloneMessage, canvas.transform);

            original.GetComponent<StaticMessage>().Type = l.type;
            clone.GetComponent<StaticMessage>().Type = l.type;

            original.GetComponentInChildren<TextMeshProUGUI>().text = l.line;
            clone.GetComponentInChildren<TextMeshProUGUI>().text = l.line;

            original.transform.SetParent(content.transform);
            clone.transform.SetParent(cloneContent.transform);

            clone.GetComponent<StaticClonedMessage>().ghost = original;
        }
    }
}
