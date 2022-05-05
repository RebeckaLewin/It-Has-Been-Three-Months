using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//adds pictures to the photoview
//this is done at runtime, as new photos can be added at anytime
public class PhotoviewParser : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject content;

    [Header("PHOTOS")]
    [SerializeField] private GameObject photoPrefab;
    [SerializeField] private GameObject fillerPrefab;
    [SerializeField] private GameObject targetContent;
    private List<GameObject> photosInRoll = new List<GameObject>();

    private PhotoviewViewer viewer;

    private void OnEnable()
    {
        SpawnPhotos();
        viewer = GetComponent<PhotoviewViewer>();
    }

    private void SpawnPhotos()
    {
        CleanUp();
        FindPhotos();

        GameObject filler = Instantiate(fillerPrefab, canvas.transform);
        filler.transform.SetParent(targetContent.transform);

        for (int i = 0; i < photosInRoll.Count; i++)
        {
            Image img = photosInRoll[i].transform.GetChild(0).GetComponent<Button>().image;

            GameObject currentPhoto = Instantiate(photoPrefab, canvas.transform);
            currentPhoto.transform.SetParent(targetContent.transform);

            Button photoBtn = currentPhoto.transform.GetChild(0).GetComponent<Button>();
            photoBtn.onClick.AddListener(() => viewer.SetImage(img));

            Image targetImage = photoBtn.image;
            targetImage.sprite = img.sprite;
            targetImage.SetNativeSize();
        }


        filler = Instantiate(fillerPrefab, canvas.transform);
        filler.transform.SetParent(targetContent.transform);

        LayoutRebuilder.ForceRebuildLayoutImmediate(targetContent.GetComponent<RectTransform>());
    }

    private void FindPhotos()
    {
        List<GameObject> days = new List<GameObject>();
        for(int i = 0; i < content.transform.childCount; i++)
        {
            GameObject currentChild = content.transform.GetChild(i).gameObject;
            if (currentChild.name.Contains("day"))
                days.Add(currentChild);
        }

        foreach(GameObject day in days)
        {
            GameObject photos = day.transform.Find("photos").gameObject;
            for(int i = 0; i < photos.transform.childCount; i++)
                photosInRoll.Add(photos.transform.GetChild(i).gameObject);
        }
    }

    private void CleanUp()
    {
        RectTransform[] rts = targetContent.GetComponentsInChildren<RectTransform>();
        foreach (RectTransform rect in rts)
        {
            if (rect != rts[0])
                Destroy(rect.gameObject);
        }

        photosInRoll.Clear();
    }
}
