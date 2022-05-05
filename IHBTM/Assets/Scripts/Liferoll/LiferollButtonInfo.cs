using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//a class to store info about liferoll pictures
public class LiferollButtonInfo : MonoBehaviour
{
    public string description;
    public int likes;
    [HideInInspector] public Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }
}
