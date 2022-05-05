using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StaticClonedMessage : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ghost;
    private float speed = 10;
    private RectTransform rt;
    MessageTypes type;
    Image img;
    TextMeshProUGUI text;
    float xPos;
    DialogueManager dm;
    [SerializeField] private Sprite rSprite, sSprite;
    [SerializeField] private Color rColor, sColor;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        text = transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>();
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        type = GetComponent<StaticMessage>().Type;

        ContinueSetUpBox();
    }

    private void Update()
    {
        FollowGhost();
    }

    void ContinueSetUpBox()
    {
        switch (type)
        {
            case MessageTypes.timestamp:
                break;
            case MessageTypes.recieved:
                xPos = 10;
                GetComponent<Image>().sprite = rSprite;
                TextMeshProUGUI rt = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                rt.color = rColor;
                rt.GetComponent<RectTransform>().anchoredPosition = new Vector2(3, 0);
                break;
            case MessageTypes.sent:
                xPos = 600;
                GetComponent<Image>().sprite = sSprite;
                TextMeshProUGUI st = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                st.color = sColor;
                st.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5, 0);
                break;
            default:
                Debug.Log("Why");
                break;
        }
    }

    void FollowGhost()
    {
        float targetY = ghost.GetComponent<RectTransform>().anchoredPosition.y;
        Vector3 targetPos = new Vector3(xPos, targetY, 0);
        rt.anchoredPosition = Vector3.Lerp(rt.anchoredPosition, targetPos, Time.deltaTime * speed);
    }
}
