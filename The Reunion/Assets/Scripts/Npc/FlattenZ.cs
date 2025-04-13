using UnityEngine;

public class FlattenZ : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.z = 0f;
        transform.position = currentPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
