using UnityEngine;

[System.Serializable]
public class Clue
{
    public string clueName;
    public Sprite icon;
    public GameObject gameObject;

    [TextArea]
    public string description;

    public Clue(string name, Sprite iconSprite, string descriptionText)
    {
        clueName = name;
        icon = iconSprite;
        description = descriptionText;
        gameObject = null; // Not needed for puzzle clues
    }
}
