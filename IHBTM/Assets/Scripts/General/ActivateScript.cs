using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//activates the objects in the list, to initialize things such as chats
public class ActivateScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToActivate = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject o in objectsToActivate)
        {
            o.SetActive(true);
        }
    }
}
