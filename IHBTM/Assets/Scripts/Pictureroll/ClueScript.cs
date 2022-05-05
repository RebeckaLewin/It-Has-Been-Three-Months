using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used to store a value in a gameobject
public class ClueScript : MonoBehaviour
{
    [SerializeField] private int index;
    
    public int Index { get { return index; } }
}
