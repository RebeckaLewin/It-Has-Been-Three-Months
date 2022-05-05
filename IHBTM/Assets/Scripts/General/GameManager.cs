using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the flow of the game, as well as middlemanaging between different scripts
public class GameManager : MonoBehaviour
{
    public List<GameObject> dialogueManagers = new List<GameObject>();

    [SerializeField] Canvas canvas;

    [SerializeField] private GameObject endScreen;

    public static bool GameStarted;
    private bool firstMessageSent;

    public static bool CameraMode = false;
    [SerializeField] private GameObject screenshotter;
    [SerializeField] ScreenShotHandler ssh;
    [SerializeField] GameObject flash;

    private GameObject dot; //aid to visualized rays in 2D space

    private void Start()
    {
        GameStarted = false;
    }

    private void Update()
    {
        //sends the first message of the game
        if(GameStarted && !firstMessageSent)
        {
            StartCoroutine(SendFirstMessage());
            firstMessageSent = true;
        }
    }

    private IEnumerator SendFirstMessage()
    {
        yield return new WaitForSeconds(5f);
        dialogueManagers[0].GetComponent<DialogueManager>().SendNotice(true);  
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3f);
        endScreen.SetActive(true);
    }

    #region Camera methods
    //The GameManager works as a middleman between the screenshot mode and the actual screenshotter here
    //it checks if any clues are on screen, and captured by the camera
    public void TakeScreenshot(RectTransform rt)
    {
        int index = -1;
        Vector2 scaledRT = Vector2.Scale(rt.sizeDelta, transform.lossyScale);
        float height = scaledRT.y;
        float width = scaledRT.x;
        Vector3 pos = new Vector3(0, 0, Camera.main.transform.position.z);

        //sends out rays in a square area
        //since the recttransform is twisted due to player being able to spin it around, this function needs to account for different scales
        if(rt.localScale.x > 0)
        {
            for (float i = rt.position.x; i < width + rt.position.x; i += 0.2f)
            {
                pos.x = i;
                if (index != -1) //means that something of value has been found
                    break;

                if (rt.localScale.y < 0)
                {
                    for (float j = rt.position.y; j < height + rt.position.y; j += 0.2f)
                    {
                        pos.y = j;
                        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                        //Instantiate(dot, pos, dot.transform.rotation, FindObjectOfType<Canvas>().transform);

                        if (hit.collider != null)
                        {
                            index = hit.collider.gameObject.GetComponent<ClueScript>().Index;
                            break;
                        }
                    }
                }
                else
                {
                    for (float j = rt.position.y; j > rt.position.y - height; j -= 0.2f)
                    {
                        pos.y = j;
                        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                        //Instantiate(dot, pos, dot.transform.rotation, FindObjectOfType<Canvas>().transform);

                        if (hit.collider != null)
                        {
                            index = hit.collider.gameObject.GetComponent<ClueScript>().Index;
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            for (float i = rt.position.x; i > rt.position.x - width; i -= 0.2f)
            {
                pos.x = i;
                if (index != -1)
                    break;

                if (rt.localScale.y < 0)
                {
                    for (float j = rt.position.y; j < height + rt.position.y; j += 0.2f)
                    {
                        pos.y = j;
                        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                        //Instantiate(dot, pos, dot.transform.rotation, FindObjectOfType<Canvas>().transform);

                        if (hit.collider != null)
                        {
                            index = hit.collider.gameObject.GetComponent<ClueScript>().Index;
                            break;
                        }
                    }
                }
                else
                {
                    for (float j = rt.position.y; j > rt.position.y - height; j -= 0.2f)
                    {
                        pos.y = j;
                        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                        //Instantiate(dot, pos, dot.transform.rotation, FindObjectOfType<Canvas>().transform);

                        if (hit.collider != null)
                        {
                            index = hit.collider.gameObject.GetComponent<ClueScript>().Index;
                            break;
                        }
                    }
                }
            }
        }

        //takes the screenshot
        StartCoroutine(ssh.RecordFrame(index, rt));
        StartCoroutine(TurnOnFlash());
        CameraMode = false;
    }

    public void ToggleCameraMode()
    {
        if (CameraMode)
        {
            screenshotter.SetActive(false);
            CameraMode = false;
        }

        else
        {
            screenshotter.SetActive(true);
            CameraMode = true;
        }
    }

    IEnumerator TurnOnFlash()
    {
        yield return new WaitForFixedUpdate();
        flash.SetActive(true);
    }
    #endregion

    #region Outdated methods
    //this is the old screenshot method, remaining in the project in order to change back to the old system should the need arise
    /*
    public void TakeScreenshot()
    {
        if (!flash.activeInHierarchy)
        {
            int index = -1;
            float height = 2f * Camera.main.orthographicSize;
            float width = height * Camera.main.aspect;
            Vector3 pos = new Vector3(0, 0, Camera.main.transform.position.z);

            for(int i = (int) -(width / 2); i < width / 2; i++)
            {
                pos.x = i;
                if (index != -1)
                    break;

                for (int j = (int) -(height / 2); j < height / 2; j++)
                {
                    pos.y = j;
                    RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                    if (hit.collider != null)
                    {
                        index = hit.collider.gameObject.GetComponent<ClueScript>().Index;
                        break;
                    }
                }
            }

            /*
            if(objectsOnScreen.Count > 0 && objectsOnScreen[0] != null)
            {
                Transform objectTransform = objectsOnScreen[0].transform;
                Vector3 newPos = new Vector3(objectTransform.position.x, objectTransform.position.y, Camera.main.transform.position.z);
                RaycastHit2D hit = Physics2D.Raycast(newPos, Vector2.zero);
                if (hit.collider.gameObject == objectsOnScreen[0])
                {
                    index = objectsOnScreen[0].GetComponent<ClueScript>().Index;
                }

            }

            //StartCoroutine(ssh.RecordFrame(index));
            //StartCoroutine(TurnOnFlash());
        }
    }*/
    #endregion
}
