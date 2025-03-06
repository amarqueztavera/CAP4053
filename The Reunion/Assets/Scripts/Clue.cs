using UnityEngine;

[System.Serializable]
public class Clue
{
    public string clueName; // Name of the clue
    public Sprite icon; // Icon to display in the inventory
    public GameObject gameObject; // Reference to the clue in the game world
    [TextArea] // Makes the description field multi-line in the Inspector
    public string description; // Description of the clue
}