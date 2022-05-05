using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeScript : MonoBehaviour
{
    [SerializeField] private float time;

    private void Awake()
    {
        StartCoroutine(TurnOff());
    }

    private IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
