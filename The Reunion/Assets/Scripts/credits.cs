using UnityEngine;
using UnityEngine.UI;

public class credits : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created d d    
    public float scrollSpeed = 40f;    
    private RectTransform rectTransform;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
    }
}
