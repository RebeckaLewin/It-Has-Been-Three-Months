using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//manages the functionality of the cameraroll app
public class CameraRollManager : MonoBehaviour
{
    [SerializeField] private GameObject pictureView;
    [SerializeField] private GameObject content;
    private PhotoviewViewer viewer;
    private List<GameObject> pictures = new List<GameObject>();

    private void Start()
    {
        if (transform.parent.name != "cameraroll")
            return;

        viewer = pictureView.GetComponent<PhotoviewViewer>();
        FindPictures();
        for(int i = 0; i < 3; i++)
        {
            int index = i;
            Button btn = pictures[index].transform.GetChild(0).GetComponent<Button>();
            btn.onClick.AddListener(() => SetPictureViewActive((btn)));
        }
    }

    private void FindPictures()
    {
        List<GameObject> days = new List<GameObject>();
        for (int i = 0; i < content.transform.childCount; i++)
        {
            GameObject currentChild = content.transform.GetChild(i).gameObject;
            if (currentChild.name.Contains("day"))
                days.Add(currentChild);
        }

        foreach (GameObject day in days)
        {
            GameObject photos = day.transform.Find("photos").gameObject;
            for (int i = 0; i < photos.transform.childCount; i++)
            {
                pictures.Add(photos.transform.GetChild(i).gameObject);
            }
        }
    }

    public void SetPictureViewActive(Button btn)
    {
        pictureView.SetActive(true);
        HomeScreenManager.CurrentScreen = pictureView;
        viewer.SetImage(btn.image);
    }
}
