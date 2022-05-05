using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a gate condition
//reason as for why this is not just a bool, is for the additional string to remember which condition is what
[CreateAssetMenu]
[System.Serializable]
public class GateCondition : ScriptableObject
{
    public string condition;
    public bool isTrue = false;

    private void OnEnable()
    {
        isTrue = false;
    }
}
