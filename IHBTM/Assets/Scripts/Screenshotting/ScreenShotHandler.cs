using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//responsible for creating pictures through the camera and textures, and saves it to the cameraroll
public class ScreenShotHandler : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    Texture2D cameraTexture, newTexture;
    Rect rect;

    [SerializeField] private RectTransform anchor; //actually the mouseposition, but translated to UI-coordinates

    private bool firstScreenshot = true;
    private GameObject clone;
    [SerializeField] GameObject photo, content, todayContent;
    public Sprite sprite;


    [SerializeField] private CameraRollManager CRM;

    //records the current frame, and crops it to reflect the selected area
    public IEnumerator RecordFrame(int index, RectTransform rt)
    {
        yield return new WaitForEndOfFrame();
        cameraTexture = ScreenCapture.CaptureScreenshotAsTexture();
        cameraTexture.Apply();
        
        rect = new Rect(rt.anchoredPosition.x, rt.anchoredPosition.y, rt.sizeDelta.x, rt.sizeDelta.y);
        newTexture = new Texture2D((int)rect.width, (int)rect.height);

        int xPos = 0;
        int yPos = 0;

        if (rt.localScale.x < 0)
            xPos = (int)(anchor.anchoredPosition.x * canvas.scaleFactor);
        else
            xPos = (int)(rt.anchoredPosition.x * canvas.scaleFactor);

        if (rt.localScale.y > 0)
            yPos = (int)(anchor.anchoredPosition.y * canvas.scaleFactor);
        else
            yPos = (int)(rt.anchoredPosition.y * canvas.scaleFactor);

        newTexture.SetPixels(cameraTexture.GetPixels(xPos, yPos, (int) rect.width, (int) rect.height));
        newTexture.Apply();
        SaveScreenshot(index);

        //old attempt, kept in case designers wants to go back
        /*
        cameraTexture.ReadPixels(rect, 0, 0);
        cameraTexture.Apply();
        SaveScreenshot(index);

        newTexture.SetPixels(cameraTexture.GetPixels((int)rt.anchoredPosition.x, (int)rt.anchoredPosition.y, (int)rt.rect.width, (int)rt.rect.height));
        newTexture.Apply();
        SaveScreenshot(index);*/
    }

    //saves the screenshot to the cameraroll
    public void SaveScreenshot(int index)
    {
        if (firstScreenshot) //if it is the first photo of the day, a container needs to be added for the newly taken pictures
        {
            clone = Instantiate(todayContent, canvas.transform) as GameObject;
            clone.transform.SetParent(content.transform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
            firstScreenshot = false;
        }

        sprite = Sprite.Create(newTexture, new Rect(0, 0, (int) rect.width, (int) rect.height), Vector2.zero);
        GameObject currentPhoto = Instantiate(photo, canvas.transform) as GameObject;
        currentPhoto.GetComponent<PictureInfo>().Index = index;

        Button photoBtn = currentPhoto.transform.GetChild(0).GetComponent<Button>();
        photoBtn.onClick.AddListener(() => CRM.SetPictureViewActive(photoBtn));

        Image photoImage = photoBtn.image;
        photoImage.sprite = sprite;
        photoImage.SetNativeSize();
        photoImage.transform.localScale *= 3f;

        currentPhoto.transform.SetParent(clone.transform.GetChild(1).transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(clone.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
    }
}
