using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//manages the liferoll app
public class LiferollManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> pages = new List<GameObject>();
    private GameObject currentPage;

    //goes to a private page
    public void GoToPage(int index)
    {
        currentPage = pages[index];
        currentPage.SetActive(true);
        HomeScreenManager.CurrentScreen = currentPage;
    }

    //enters the picture view, and sets its data
    public void GoToView(LiferollButtonInfo info)
    {
        GameObject view = currentPage.transform.Find("view").gameObject;
        view.SetActive(true);
        view.transform.Find("description").GetComponent<TextMeshProUGUI>().text = info.description;
        view.transform.Find("likes").GetComponent<TextMeshProUGUI>().text = info.likes.ToString() + " Likes";
        view.transform.Find("picture").GetComponent<Image>().sprite = info.image.sprite;
        HomeScreenManager.CurrentScreen = view;
    }

    //goes back to main feed
    public void GoBackHome()
    {
        if (HomeScreenManager.CurrentScreen == this.gameObject)
            return;

        if (currentPage.transform.Find("view").gameObject.activeInHierarchy)
            currentPage.transform.Find("view").gameObject.GetComponent<Transition>().Exit();

        currentPage.GetComponent<Transition>().Exit();
        currentPage = null;
        HomeScreenManager.CurrentScreen = this.gameObject;
    }
}
