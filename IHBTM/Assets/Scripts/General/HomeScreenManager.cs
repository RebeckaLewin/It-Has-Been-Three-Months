using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attaches to the Homescreen, and is responsible for navigating apps
public class HomeScreenManager : MonoBehaviour
{
    public List<GameObject> appScreens = new List<GameObject>();
    private GameObject currApp;
    public static GameObject CurrentScreen;

    [SerializeField] private GameObject canvas;
    [SerializeField] private GameManager gm;

    //starts the app selected
    public void GoToApp(int index)
    {
        appScreens[index].SetActive(true);
        currApp = appScreens[index];
        GameObject a = appScreens[index];

        //checks if any child screens are active
        bool noActiveChildren;
        noActiveChildren = FindActiveChild(a);
        if (noActiveChildren)
        {
            CurrentScreen = appScreens[index];
        }
    }

    //loops through the app's screens and their respective children to find active screen
    private bool FindActiveChild(GameObject p)
    {
        bool noActiveChildren = false;
        for (int i = p.transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = p.transform.GetChild(i).gameObject;
            bool noChildScreensActive = true;
            if (child.activeSelf && child.CompareTag("Screen")) //if an active screen is found, either go back or check that screen's children
            {
                CurrentScreen = child;
                if(child.transform.childCount != 0)
                {
                    noChildScreensActive = FindActiveChild(child);
                }
                break;
            }
            if (i == 0 && noChildScreensActive)
            {
                noActiveChildren = true;
            }
        }
        return noActiveChildren;
    }

    //goes back to the homescreen
    public void BackToHomeScreen()
    {
        if(currApp != null && currApp.activeSelf == true)
        {
            currApp.GetComponent<Transition>().Exit();
        }
    }

    //the functionality of the backtrack-button, goes back to previous screen
    public void Backtrack()
    {
        if(CurrentScreen != null && CurrentScreen.activeSelf == true)
        {
            GameObject prev = CurrentScreen;
            if (prev.GetComponent<Transition>() != null)
                prev.GetComponent<Transition>().Exit();
            if(CurrentScreen.transform.parent.gameObject == canvas)
            {
                CurrentScreen = null;
            }
            else
            {
                CurrentScreen = CurrentScreen.transform.parent.gameObject;
            }
        }

        //an event in the story
        if(gm.dialogueManagers[0].GetComponent<DialogueManager>().currentGateIndex == 2 && CurrentScreen == appScreens[4])
        {
            gm.dialogueManagers[0].GetComponent<DialogueManager>().OpenGate(2);
        }
    }

    //can be called from other classes to change the current screen
    public void SetCurrentScreen(GameObject screen)
    {
        CurrentScreen = screen;
    }
}
