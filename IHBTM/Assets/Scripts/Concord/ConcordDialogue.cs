using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class ConcordDialogue : ScriptableObject
{
    public List<ConcordLine> dialogue = new List<ConcordLine>();
}
