using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//handles formatting for the photoview
public class PhotoviewViewer : MonoBehaviour
{
    [SerializeField] private Image targetImage;

    [SerializeField] private GameObject photoPrefab;
    [SerializeField] private GameObject photoList;
    [SerializeField] private Sprite normalFrame, selectedFrame;

    private float widthOfPhoto = 56;

    public void SetImage(Image img)
    {
        targetImage.sprite = img.sprite;
        targetImage.SetNativeSize();

        StartCoroutine(CenterSelected());
    }

    private IEnumerator CenterSelected()
    {
        yield return new WaitForEndOfFrame();
        for(int i = 1; i < photoList.transform.childCount - 1; i++)
        {
            GameObject actualPhoto = photoList.transform.GetChild(i).transform.GetChild(0).gameObject;
            GameObject frame = photoList.transform.GetChild(i).transform.GetChild(1).gameObject;

            if (actualPhoto.GetComponent<Button>().image.sprite == targetImage.sprite)
            {
                RectTransform photoListRT = photoList.GetComponent<RectTransform>();

                Vector3 newPos = new Vector3(-((photoListRT.rect.width / 2) + ((widthOfPhoto * i) - (widthOfPhoto * 0.5f))), photoListRT.anchoredPosition.y);
                photoListRT.anchoredPosition = newPos;

                frame.GetComponent<Image>().sprite = selectedFrame;
                continue;
            }

            frame.GetComponent<Image>().sprite = normalFrame;
        }
    }
}
