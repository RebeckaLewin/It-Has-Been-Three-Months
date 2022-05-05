using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a class to store information in the pictures
public class PictureInfo : MonoBehaviour
{
    [SerializeField] private int index = -1;
    private TemporaryDialogue dialogue = null;
    public bool HasBeenSent { get; set; }
    public bool HasExpired { get; set; }

    public int Index { get { return index; } set { index = value; } }
    
    public void SetDialogue(TemporaryDialogue blocks)
    {
        dialogue = blocks;
    }

    public TemporaryDialogue GetDialogue()
    {
        return dialogue;
    }
}
