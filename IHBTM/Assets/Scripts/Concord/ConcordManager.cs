using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//manages the functionality of the concord app
public class ConcordManager : MonoBehaviour
{
    public List<GameObject> servers = new List<GameObject>();
    private GameObject currentServer;
    [SerializeField] private RectTransform circleRect;

    private void Start()
    {
        currentServer = servers[2];
        circleRect.transform.position = currentServer.GetComponent<RectTransform>().anchoredPosition;
    }

    public void ChangeServer(int index)
    {
        currentServer.SetActive(false);
        currentServer = servers[index];
        currentServer.SetActive(true);
        circleRect.anchoredPosition = currentServer.GetComponent<RectTransform>().anchoredPosition;
    }
}
