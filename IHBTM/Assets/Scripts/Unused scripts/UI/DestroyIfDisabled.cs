using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfDisabled : MonoBehaviour
{
    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
