using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//aid to translating the mouse position and texture coordinates
public class AnchorScript : MonoBehaviour
{
    private Vector2 mousePos;

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;
    }
}
