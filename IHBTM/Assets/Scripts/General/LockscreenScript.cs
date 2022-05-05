using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//functionality of the lockscreen, mostly for animation
public class LockscreenScript : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private Canvas canvas;

    public void MoveOutOfTheWay()
    {
        LeanTween.moveLocalX(this.gameObject, -608, time).setEaseOutCirc();
        GameManager.GameStarted = true;
        foreach (Transform screen in canvas.transform.GetComponentInChildren<Transform>())
        {
            if (screen.gameObject == this.gameObject)
                return;
            LeanTween.moveLocalX(screen.gameObject, 0, time).setEaseOutCirc();
        }
    }
}
