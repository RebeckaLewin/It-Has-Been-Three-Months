using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//functionality for the notice object
public class NoticeScript : MonoBehaviour
{
    [SerializeField] private float timeBeforeDisable, transitionTime;

    private void OnEnable()
    {
        LeanTween.moveLocalY(this.gameObject, 470, transitionTime).setEaseOutCirc();
        StartCoroutine(Close());
    }

    private IEnumerator Close()
    {
        yield return new WaitForSeconds(timeBeforeDisable);
        LeanTween.moveLocalY(this.gameObject, 600, transitionTime).setEaseInCirc();
        StartCoroutine(TurnOff());

    }

    private IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(transitionTime);
        gameObject.SetActive(false);
    }
}
