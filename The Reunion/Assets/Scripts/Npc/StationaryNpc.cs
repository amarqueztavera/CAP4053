using Kinnly;
using UnityEngine;

public class StationaryNpc : MonoBehaviour
{


    public string[] randomDialogue;

    [Header("Conditional Dialogues")]
    public string[] act1Dialogue;
    public string[] act2Dialogue;
    public string[] act3Dialogue;

    private int condition1Index = 0;
    private int condition2Index = 0;
    private int condition3Index = 0;

    private bool playerInRange = false;

    [SerializeField] Transform player;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, new Vector3(player.position.x, player.position.y, 0));
        if (distanceToPlayer<=1 && Input.GetKeyDown(KeyCode.E))
        {
            Speak();
        }
    }

    void Speak()
    {
        string lineToSay = "";


        //check for act 3 dialoge
        if (NPCStateManager.Instance.act3 && act3Dialogue.Length > 0)
        {
            lineToSay = act3Dialogue[condition3Index];
            condition3Index = (condition3Index + 1) % act3Dialogue.Length;
        }

        //check for act 2 dialoge
        else if (NPCStateManager.Instance.act2 && act2Dialogue.Length > 0)
        {
            lineToSay = act2Dialogue[condition2Index];
            condition2Index = (condition2Index + 1) % act2Dialogue.Length;
        }
        
        //check for act 3 dialoge
        else if (NPCStateManager.Instance.act1 && act1Dialogue.Length > 0)
        {
            lineToSay = act1Dialogue[condition1Index];
            condition1Index = (condition1Index + 1) % act1Dialogue.Length;
        }

        //random dialoge
        else if (randomDialogue.Length > 0)
        {
            int randomIndex = Random.Range(0, randomDialogue.Length);
            lineToSay = randomDialogue[randomIndex];
        }

        Debug.Log("NPC says: " + lineToSay);
        // Replace this with actual dialogue UI logic
    }

    void OnMouseDown()
    {
        Debug.Log("SPEAKKKK");
        Speak();
    }

}
