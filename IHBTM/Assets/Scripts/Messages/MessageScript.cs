using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//formats the message box
public class MessageScript : MonoBehaviour
{
    TextMeshProUGUI text;
    private MessageTypes type;
    Image img;
    public Sprite CurrentSprite { get; set; }
    RectTransform rt;

    public MessageTypes Type { get { return type; } set { type = value; } }
    
    void Start()
    {
        rt = GetComponent<RectTransform>();
        text = transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>();
        img = GetComponent<Image>(); 

        SetUp();
        SetUpBox();
    }

    void SetUp()
    {
        if(CurrentSprite != null)
        {
            transform.GetChild(1).GetComponent<Image>().sprite = CurrentSprite;
        }

        switch (type)
        {
            case MessageTypes.recieved:
                rt.pivot = new Vector2(0f, 0.5f);
                text.alignment = TextAlignmentOptions.Left;
                break;
            case MessageTypes.sent:
                rt.pivot = new Vector2(1f, 0.5f);
                text.alignment = TextAlignmentOptions.Left;
                break;
        }
    }

    void SetUpBox()
    {
        if (CurrentSprite == null)
        {
            Vector2 preferredValues = text.GetPreferredValues(float.PositiveInfinity, float.PositiveInfinity);
            float w = preferredValues.x;
            float h = preferredValues.y;

            if (text.preferredWidth < 400)
            {
                text.rectTransform.sizeDelta = new Vector2(w, h);
            }
            else
            {
                text.rectTransform.sizeDelta = new Vector2(400, 0);
                text.rectTransform.sizeDelta = new Vector2(400, text.preferredHeight);
            }

            Vector3 size = text.rectTransform.sizeDelta + new Vector2(40, 20);

            rt.sizeDelta = size;
        }
        else
        {
            rt.sizeDelta = new Vector2(302, 302);
        }
    }
}
