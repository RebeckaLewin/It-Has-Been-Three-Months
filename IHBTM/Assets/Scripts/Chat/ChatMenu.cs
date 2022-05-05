using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles the functionality of the chatmenu
public class ChatMenu : MonoBehaviour
{
    [SerializeField] private GameObject hs;
    private HomeScreenManager hsm;
    [SerializeField] private List<GameObject> chats = new List<GameObject>();

    private void Awake()
    {
        hsm = hs.GetComponent<HomeScreenManager>();
    }

    public void SelectChatRoom(int index)
    {
        chats[index].SetActive(true);
        hsm.SetCurrentScreen(chats[index]);
    }
}
