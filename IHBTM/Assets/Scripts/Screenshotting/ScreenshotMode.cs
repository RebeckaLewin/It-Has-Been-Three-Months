using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//adds the functionality of the screenshot mode, and communicates to the GameManager *when* to take a screenshot
public class ScreenshotMode : MonoBehaviour
{
    public GameObject square;
    private RectTransform currentRT;
    private bool centering;
    private Vector2 origin;
    private Vector2 mousePos;
    [SerializeField] private GameManager gm;

    private float xTarget, yTarget;
    private float xSize, ySize;

    private Vector2 view;
    private bool isOutside;

    private CanvasGroup cg;

    Vector2 mousePosScreen;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        cg.alpha = 0;
        LeanTween.alphaCanvas(cg, 1, 0.5f);
    }


    private void Update()
    {
        if (!GameManager.CameraMode)
            return;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonUp(0))
        {
            centering = false;
            if (isOutside) //resets if mouse is outside of screen
            {
                Destroy(currentRT.gameObject);
                currentRT = null;
                cg.alpha = 0;
                LeanTween.alphaCanvas(cg, 1, 1f);
                return;
            }

            gm.TakeScreenshot(currentRT); //takes the screenshot
            currentRT.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            StartCoroutine(DestroySquare(currentRT.gameObject));
        }

        //checks if mouse is inside screen
        view = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;

        if (isOutside)
            return;

        //spawns a square at mouse position at click
        if(Input.GetMouseButtonDown(0))
        {
            GameObject currentSquare = Instantiate(square, mousePos, square.transform.rotation, GameObject.FindObjectOfType<Canvas>().transform);
            currentRT = currentSquare.GetComponent<RectTransform>();
            currentRT.sizeDelta = Vector2.zero;
            origin = Camera.main.WorldToScreenPoint(mousePos);
            LeanTween.alphaCanvas(cg, 0, 0.25f);
            centering = true;
        }

        //formats the square depending on mouse position, allowing player to select where to screenshot
        else if(centering == true)
        {
            if (Input.mousePosition.x < origin.x)
            {
                xTarget = origin.x - Input.mousePosition.x;
                xSize = -1;
            }
            else
            {
                xTarget = Input.mousePosition.x - origin.x;
                xSize = 1;
            }

            if(Input.mousePosition.y > origin.y)
            {
                yTarget = Input.mousePosition.y - origin.y;
                ySize = -1;
            }
            else
            {
                yTarget = origin.y - Input.mousePosition.y;
                ySize = 1;
            }

            currentRT.localScale = new Vector3(xSize, ySize, currentRT.localScale.z);

            Vector2 targetPos = new Vector2(xTarget, yTarget) * 1.25f;
            currentRT.sizeDelta = Vector2.Lerp(currentRT.sizeDelta, targetPos, 0.9f);
        }
    }

    //destroys the square, and goes back to normal gameplay
    private IEnumerator DestroySquare(GameObject square)
    {
        yield return new WaitForEndOfFrame();
        Destroy(square);
        currentRT = null;
        gameObject.SetActive(false);
    }
}
