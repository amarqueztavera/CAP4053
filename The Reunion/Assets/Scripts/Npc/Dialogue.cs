using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    private string[] dialogue;
    private int index;

    public GameObject continueButton;
    public float wordSpeed;
    public bool playerIsClose;

    public bool canTalk= true;

    public string[] randomDialogue;

    [Header("Conditional Dialogues")]
    public string[] act1Dialogue;
    public string[] act2Dialogue;
    public string[] act3Dialogue;

    //private int condition1Index = 0;
    //private int condition2Index = 0;
    //private int condition3Index = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (NPCStateManager.Instance.act3 == true && act3Dialogue.Length > 0)
        {
            canTalk = true;
            dialogue = act3Dialogue;
        }
        else if (NPCStateManager.Instance.act2 == true && act2Dialogue.Length > 0)
        {
            canTalk = true;
            dialogue = act2Dialogue;
        }
        else if (NPCStateManager.Instance.act1 == true && act1Dialogue.Length > 0)
        {
            canTalk = true;
            dialogue = act1Dialogue;
        }
        else if (randomDialogue.Length > 0)
        {
            canTalk=true;
            dialogue = randomDialogue;
        }
        else
            canTalk = false;

        if (Input.GetKeyUp(KeyCode.E) && playerIsClose && !NPCStateManager.Instance.maxSuspicion && canTalk)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }

        if(dialogueText.text == dialogue[index])
        {
            continueButton.SetActive(true); 
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach(char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {

        continueButton.SetActive(false) ;  
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else 
        {
            zeroText();
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerIsClose = true;

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }

    }
}
