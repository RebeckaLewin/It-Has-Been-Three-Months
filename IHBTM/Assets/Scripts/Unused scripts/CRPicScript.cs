using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRPicScript : MonoBehaviour
{
    [SerializeField] private DialogueManager dm;
    [SerializeField] private List<DialogueBlock> tempDBs = new List<DialogueBlock>();
    public bool HasBeenSent { get; set; }

    public List<DialogueBlock> GetBlocks
    {
        get { return tempDBs; }
    }
}
