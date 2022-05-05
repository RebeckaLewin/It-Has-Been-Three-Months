using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//aid for telling the manager when the chat activates and deactivates
public class ChatScript : MonoBehaviour
{
    [SerializeField] private DialogueManager dm;

    private void OnEnable()
    {
        dm.OnChatEnable();
    }

    private void OnDisable()
    {
        dm.OnChatDisable();
    }
}
