using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;

    public GameObject continueButton;
    public float wordSpeed;
    public bool playerIsClose;



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
        if (NPCStateManager.Instance.act3 == true)
            dialogue = act3Dialogue;
        else if (NPCStateManager.Instance.act2 == true)
            dialogue = act2Dialogue;
        else if(NPCStateManager.Instance.act1 == true)
            dialogue = act1Dialogue;
        

        if (Input.GetKeyUp(KeyCode.E) && playerIsClose && !NPCStateManager.Instance.maxSuspicion)
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
