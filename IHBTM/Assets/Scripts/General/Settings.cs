using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

//the settings of the game
public class Settings : MonoBehaviour
{
    public static bool isDynamic = true;

    //changes the delay between messages
    public void ChangeDelay(bool target)
    {
        isDynamic = target;
    }
}
