using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//handles everything that has to do with dialogue, including logic and flow
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject myChat;

    public List<DialogueBlock> dialogBlocks = new List<DialogueBlock>();
    public List<DialogueBlock> temporaryDialogue = new List<DialogueBlock>();
    [HideInInspector] public DialogueBlock currentBlock;

    private List<DialogueBlock> parallelBlocks = new List<DialogueBlock>();
    private List<DialogueBlock> bBlocks = new List<DialogueBlock>();

    [HideInInspector] public int blockNum;
    [HideInInspector] public int lineNum;
    [HideInInspector] public int currentGateIndex = -1;

    [Header("BUTTONS")]
    [SerializeField] private GameObject choiceButtons;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject textboxButton;
    [SerializeField] private GameObject parallelButton;
    [SerializeField] private GameObject selectPicButton;

    [Header("MESSAGES")]
    [SerializeField] private GameObject message;
    [SerializeField] private GameObject clonedMessage;
    [SerializeField] private GameObject picture;
    [SerializeField] private GameObject clonedPicture;

    [Header("PARENTS")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject duplicateContent;

    [Header("OTHER")]
    [SerializeField] private GameObject loadingSprite;
    [SerializeField] private GameObject buttonsBox;
    [SerializeField] private GameObject writingPrompt;
    [SerializeField] private GameManager gm;
    [SerializeField] private GameObject notice;
    [SerializeField] private GameObject chatBtn;

    public List<GateCondition> listOfConditions = new List<GateCondition>();

    //<Bools>
    private bool inTemporaryBlock;
    public bool inBlock;
    [SerializeField] private bool startSending = true;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        currentBlock = dialogBlocks[0];

        if (!startSending)
            return;

        if (currentBlock.lines[lineNum].type == MessageTypes.recieved)
        {
            StartCoroutine(SendingDelay());
        }
        else { StartCoroutine(WaitBetweenLines()); }
    }

    public void OnChatEnable()
    {
        if (blockNum >= dialogBlocks.Count - 1)
            return;

        if (dialogBlocks[blockNum + 1].type == DialogueBlock.BlockType.basic)
        {
            if (lineNum < currentBlock.lines.Count)
            {
                if (currentBlock.lines[lineNum].type == MessageTypes.sent)
                {
                    continueButton.SetActive(true);
                    textboxButton.SetActive(true);
                }
            }

            else if (dialogBlocks[blockNum + 1].type == DialogueBlock.BlockType.gate)
            {
                if (dialogBlocks[blockNum + 1].lines[0].type == MessageTypes.sent)
                {
                    continueButton.SetActive(true);
                    textboxButton.SetActive(true);
                }
            }
        }

        else
        {
            if (listOfConditions[dialogBlocks[blockNum + 1].conditionIndex].isTrue == true)
            {
                continueButton.SetActive(true);
                textboxButton.SetActive(true);
            }
        }

    }

    public void OnChatDisable()
    {
        StopAllCoroutines();
        if (lineNum < currentBlock.lines.Count)
        {
            if (currentBlock.lines[lineNum].type == MessageTypes.sent)
                return;

            SendMessage(myChat.activeInHierarchy);
        }
    }

    //adds delay before sending to mimic thinking
    private IEnumerator SendingDelay()
    {
        float delay = 0;
        if (currentBlock.lines[lineNum].type == MessageTypes.recieved)
        {
            if (!Settings.isDynamic)
                delay = 0.7f;
            else
            {
                delay = currentBlock.lines[lineNum].line.Length * 0.04f;
                if (delay > 3f)
                    delay = 3f;
            }
        }

        yield return new WaitForSeconds(delay);
        StartCoroutine(WaitBetweenLines());
    }

    //adds delay between the lines to mimic writing
    private IEnumerator WaitBetweenLines()
    {
        GameObject loadingSprite = null;
        float waitTime = 0f;
        if(currentBlock.lines[lineNum].type == MessageTypes.recieved
            && Settings.isDynamic)
        {
            loadingSprite = Instantiate(this.loadingSprite, canvas.transform);
            loadingSprite.transform.SetParent(content.transform);
            waitTime = currentBlock.lines[lineNum].line.Length * 0.08f;
            if (waitTime > 5f || waitTime == 0)
            {
                waitTime = 5f;
            }
        }
        yield return new WaitForSeconds(waitTime);
        if(loadingSprite != null)
            Destroy(loadingSprite);
        SendMessage(myChat.activeInHierarchy);        
    }

    //the method for sending messages
    //starts a delayed coroutine so other scripts have time to change variables before line change
    private void SendMessage(bool isActive)
    {
        if (!isActive)
        {
            int index = 0;
            int currentNum = lineNum;

            if (lineNum != 0)
            {
                while (index < (currentBlock.lines.Count - lineNum))
                {
                    if (currentBlock.lines[currentNum].type == MessageTypes.sent)
                    {
                        break;
                    }
                    index++;
                    currentNum++;
                }
            }

            else
            {
                foreach (SingleLine line in currentBlock.lines)
                {
                    if (line.type != MessageTypes.sent)
                        index++;
                    else
                        break;
                }
            }


            for (int i = 0; i < index; i++)
            {
                InstantiateMessage();
                if (i == 0 && GameManager.GameStarted)
                {
                    SendNotice(false);
                }

                lineNum++;
            }

            CheckIfNoMoreLines();
        }

        else
        {
            InstantiateMessage();
            StartCoroutine(ContinueToSend());
        }
    }

    //spawns the messages in the scene and sets their data
    private void InstantiateMessage()
    {
        GameObject originalObject = null;
        if (currentBlock.lines[lineNum].sprite != null)
        {
            originalObject = picture;
        }
        else { originalObject = message; }
        
        GameObject latestMessage = Instantiate(originalObject, canvas.transform);

        float xPos = 0;

        GameObject cloneObject = null;
        if (currentBlock.lines[lineNum].sprite != null)
        {
            cloneObject = clonedPicture;
        }
        else { cloneObject = clonedMessage; }

        GameObject theClone = Instantiate(cloneObject, new Vector3(xPos, -4), transform.rotation, canvas.transform);

        if (currentBlock.lines[lineNum].sprite != null)
        {
            latestMessage.GetComponent<MessageScript>().CurrentSprite = currentBlock.lines[lineNum].sprite;
            theClone.GetComponent<MessageScript>().CurrentSprite = currentBlock.lines[lineNum].sprite;
        }

        theClone.GetComponent<ChatMessageScript>().ghost = latestMessage;

        latestMessage.transform.SetParent(content.transform);
        theClone.transform.SetParent(duplicateContent.transform);

        TextMeshProUGUI text = latestMessage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.SetText(dialogBlocks[blockNum].lines[lineNum].line);
        MessageScript ms = latestMessage.GetComponent<MessageScript>();
        ms.Type = dialogBlocks[blockNum].lines[lineNum].type;

        TextMeshProUGUI clonedText = theClone.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        clonedText.SetText(dialogBlocks[blockNum].lines[lineNum].line);
        MessageScript clonedMS = theClone.GetComponent<MessageScript>();
        clonedMS.Type = dialogBlocks[blockNum].lines[lineNum].type;
    }

    //Increases the line number
    private IEnumerator ContinueToSend()
    {
        yield return new WaitForFixedUpdate();
        lineNum++;

        CheckIfNoMoreLines();
    }

    //checks if we have gone through all lines in the current block
    private void CheckIfNoMoreLines()
    {
        if (lineNum < currentBlock.lines.Count)
        {
            if (currentBlock.lines[lineNum].type == MessageTypes.sent)
            {
                continueButton.SetActive(true);
                textboxButton.SetActive(true);
            }
            else { StartCoroutine(SendingDelay()); }
        }

        else { CheckTypes(); }
    }

    //checks what type the next message is, and then continues dialogue
    private void CheckTypes()
    {
        if (blockNum >= dialogBlocks.Count - 1)
        {
            StartCoroutine(gm.EndGame());
            return;
        }

        if (dialogBlocks[blockNum + 1].type == DialogueBlock.BlockType.branch)
        {
            //choiceButtons.SetActive(true);
            InstantiateBranch();
            textboxButton.SetActive(true);
        }
        else if (dialogBlocks[blockNum + 1].type == DialogueBlock.BlockType.gate)
        {
            if(listOfConditions[dialogBlocks[blockNum + 1].conditionIndex].isTrue)
            {
                if (dialogBlocks[blockNum + 1].lines[0].type == MessageTypes.sent)
                {
                    continueButton.SetActive(true);
                    textboxButton.SetActive(true);
                }
                else { ContinueToNextBlock(); }
            }
            else
            {
                if(dialogBlocks[blockNum + 1].openWithPic)
                {
                    selectPicButton.SetActive(true);
                }
                currentGateIndex = dialogBlocks[blockNum + 1].conditionIndex;
            }
        }
        else if(dialogBlocks[blockNum + 1].type == DialogueBlock.BlockType.parallel)
        {
            GeneralParallel();
            textboxButton.SetActive(true);
        }
        else if(dialogBlocks[blockNum + 1].lines[0].type == MessageTypes.sent)
        {
            continueButton.SetActive(true);
            textboxButton.SetActive(true);
        }
        else { ContinueToNextBlock(); }
    }

    #region Temporary blocks
    //adds temporary blocks at runtime
    public void SendBlocks(TemporaryDialogue tempDialogue)
    {
        List<DialogueBlock> blockList = tempDialogue.ListOfBlocks;

        for(int i = blockList.Count - 1; i >= 0; i--)
        {
            dialogBlocks.Insert(blockNum + 1, blockList[i]);
        }
        ContinueToNextBlock();
    }

    #endregion

    //opens the gate
    public void OpenGate(int index)
    {
        if(index >= 0 && index < listOfConditions.Count)
            listOfConditions[index].isTrue = true;
    }

    //continues dialogue after a gate
    public void ContinueAfterGate()
    {
        inBlock = true;
        if (dialogBlocks[blockNum + 1].lines[0].type == MessageTypes.sent)
        {
            continueButton.SetActive(true);
            textboxButton.SetActive(true);
        }
        else { ContinueToNextBlock(); }
    }

    //continues to the next block
    private void ContinueToNextBlock()
    {
        selectPicButton.SetActive(false);

        blockNum++;
        currentBlock = dialogBlocks[blockNum];
        lineNum = 0;

        if(dialogBlocks[blockNum].lines[0].type == MessageTypes.sent && inTemporaryBlock)
        {
            continueButton.SetActive(true);
            textboxButton.SetActive(true);
        }

        else { StartCoroutine(SendingDelay()); }
    }

    #region Buttons
    //method for the continue button
    public void SelectContinue()
    {
        inBlock = true;
        continueButton.SetActive(false);
        if (lineNum < currentBlock.lines.Count)
        {
            StartCoroutine(SendingDelay());
        }
        else { ContinueToNextBlock(); }
    }
    #endregion

    #region Branch blocks
    //spawns the buttons for selecting a branch, and their functionality
    private void InstantiateBranch()
    {
        bBlocks.Clear();
        bBlocks = GetBranchList();

        GameObject p = null;

        for (int i = 0; i < bBlocks.Count; i++)
        {
            GameObject btn = Instantiate(parallelButton, canvas.transform);
            btn.transform.SetParent(buttonsBox.transform);
            btn.GetComponent<ParallelButton>().SetText(bBlocks[i].lines[0].line);
            var num = i;
            btn.GetComponent<Button>().onClick.AddListener(() => writingPrompt.SetActive(false));
            btn.GetComponent<Button>().onClick.AddListener(() => myChat.GetComponent<Slide>().HideOptions());
            DialogueBlock selectedBlock = bBlocks[i];
            btn.GetComponent<Button>().onClick.AddListener(() => SelectABranch(selectedBlock));
            p = btn.transform.parent.gameObject;
        }

        for (int i = 2; i < p.transform.childCount; i++)
        {
            int currentChild = i;
            GameObject btn = p.transform.GetChild(currentChild).gameObject;
            for (int j = 2; j < p.transform.childCount; j++)
            {
                int childNum = j;
                if (p.transform.GetChild(j) != btn)
                {
                    GameObject otherButtons = p.transform.GetChild(j).gameObject;
                    btn.GetComponent<Button>().onClick.AddListener(() => Destroy(otherButtons));
                }
                btn.GetComponent<Button>().onClick.AddListener(() => Destroy(btn.gameObject));
            }
        }
    }

    //get list of branch blocks
    private List<DialogueBlock> GetBranchList()
    {
        List<DialogueBlock> bBlocks = new List<DialogueBlock>();
        for (int i = blockNum + 1; i < dialogBlocks.Count; i++)
        {
            if (dialogBlocks[i].type == DialogueBlock.BlockType.branch)
            {
                bBlocks.Add(dialogBlocks[i]);
            }
            else { break; }
        }
        return bBlocks;
    }

    //method for selecting a branch, removes blocks not chosen
    private void SelectABranch(DialogueBlock block)
    {
        List<DialogueBlock> blockToRemove = new List<DialogueBlock>();

        for(int i = blockNum + 1; i < blockNum + 1 + bBlocks.Count; i++)
        {
            if(dialogBlocks[i] != block)
            {
                blockToRemove.Add(dialogBlocks[i]);
            }
        }

        for(int i = 0; i < dialogBlocks.Count; i++)
        {
            if (blockToRemove.Contains(dialogBlocks[i]))
            {
                dialogBlocks.Remove(dialogBlocks[i]);
                i--;
            }
        }
        ContinueToNextBlock();
    }
    #endregion

    #region Parallel blocks
    //spawns the buttons for selecting a parallel dialogue, and their functionality
    private void GeneralParallel()
    {
        parallelBlocks = GetParallelList(); //get a list containing 

        GameObject parent = null;

        for (int i = 0; i < parallelBlocks.Count; i++)
        {
            GameObject btn = Instantiate(parallelButton, canvas.transform);
            btn.transform.SetParent(buttonsBox.transform);
            btn.GetComponent<ParallelButton>().SetText(parallelBlocks[i].lines[0].line);
            var num = i;
            btn.GetComponent<Button>().onClick.AddListener(() => MoveAroundBlocks(num)); //the important function!
            btn.GetComponent<Button>().onClick.AddListener(() => writingPrompt.SetActive(false)); //a feedback sprite
            btn.GetComponent<Button>().onClick.AddListener(() => myChat.GetComponent<Slide>().HideOptions()); //triggers the animation
            parent = btn.transform.parent.gameObject;
        }

        for (int i = 2; i < parent.transform.childCount; i++)
        {
            int currentChild = i;
            GameObject btn = parent.transform.GetChild(currentChild).gameObject;
            for(int j = 2; j < parent.transform.childCount; j++)
            {
                int childNum = j;
                if(parent.transform.GetChild(j) != btn)
                {
                    GameObject otherButtons = parent.transform.GetChild(j).gameObject;
                    btn.GetComponent<Button>().onClick.AddListener(() => Destroy(otherButtons));
                }
                btn.GetComponent<Button>().onClick.AddListener(() => Destroy(btn.gameObject));
            }
        }
    }

    //gets a list of the parallel blocks
    private List<DialogueBlock> GetParallelList()
    {
        List<DialogueBlock> pBlocks = new List<DialogueBlock>();
        for (int i = blockNum + 1; i < dialogBlocks.Count; i++)
        {
            if (dialogBlocks[i].type == DialogueBlock.BlockType.parallel)
            {
                pBlocks.Add(dialogBlocks[i]);
            }
            else { break; }
        }
        return pBlocks;
    }

    //shuffles around blocks based on chosen button
    private void MoveAroundBlocks(int index)
    {
        for (int i = 0; i < dialogBlocks.Count; i++)
        {
            if(parallelBlocks[index] == dialogBlocks[i])
            {
                dialogBlocks.RemoveAt(i);
                dialogBlocks.Insert(blockNum + 1, parallelBlocks[index]);
                break;
            }
        }
        ContinueToNextBlock();
    }
    #endregion

    #region Notice methods

    //sets the notice active
    public void SendNotice(bool first)
    {
        string noticeLine = "Unknown number : ";
        if (!first)
            noticeLine += currentBlock.lines[lineNum].line;
        else
            noticeLine += dialogBlocks[0].lines[0].line;

        notice.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = noticeLine;
        notice.SetActive(true);
        chatBtn.GetComponent<ChatbtnTransition>().Format();
    }
    #endregion
}
