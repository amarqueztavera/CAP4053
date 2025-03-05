using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public RawImage mapRawImage; // Reference to the UI RawImage
    public Camera gridCamera; // Reference to the GridCamera
    public GameObject suspicionMeter;
    private bool isMapOpen = false;

    void Start()
    {
        // Disable the map initially
        mapRawImage.gameObject.SetActive(false);
        gridCamera.gameObject.SetActive(false); // Disable camera when not in use
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }

    void ToggleMap()
    {
        isMapOpen = !isMapOpen;
        mapRawImage.gameObject.SetActive(isMapOpen);
        gridCamera.gameObject.SetActive(isMapOpen); // Re-enable camera when needed

        // Force the camera to render the grid
        if (isMapOpen)
        {
            gridCamera.Render(); // Explicitly render the grid
            suspicionMeter.SetActive(false);
        } else
        {
            suspicionMeter.SetActive(true);
        }

        // Pause gameplay
        Time.timeScale = isMapOpen ? 0f : 1f;
    }
}