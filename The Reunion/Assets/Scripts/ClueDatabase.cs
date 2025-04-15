using System.Collections.Generic;
using UnityEngine;

public class ClueDatabase : MonoBehaviour
{
    public static ClueDatabase Instance;

    public List<Clue> allClues; // Add ALL possible clues here in inspector

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Clue GetClueByName(string clueName)
    {
        return allClues.Find(c => c.clueName == clueName);
    }
}
