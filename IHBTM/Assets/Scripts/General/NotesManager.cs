using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotesManager : MonoBehaviour
{
    [SerializeField] private GameObject notesview;
    [SerializeField] private Image bg;
    [SerializeField] private TextMeshProUGUI title, content;

    public void GoToView(Button clickedButton)
    {
        notesview.SetActive(true);
        HomeScreenManager.CurrentScreen = notesview;

        GameObject note = clickedButton.gameObject;
        bg.color = note.GetComponent<Image>().color;
        title.text = note.transform.Find("title").GetComponent<TextMeshProUGUI>().text;
        content.text = note.transform.Find("content").GetChild(0).GetComponent<TextMeshProUGUI>().text;
    }
}
