using UnityEngine;
using UnityEngine.SceneManagement;

public class ClueReviewManager : MonoBehaviour
{
    public void ReturnToFinalChoice()
    {
        SceneManager.LoadScene("End Scene");
    }
}
