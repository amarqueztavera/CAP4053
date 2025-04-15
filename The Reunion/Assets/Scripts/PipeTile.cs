using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PipeTile : MonoBehaviour, IPointerClickHandler
{
    public enum PipeType { Straight, Elbow, Cross }
    public PipeType pipeType;

    public Image tileImage; // Assign in Inspector

    public Sprite straightSprite;
    public Sprite elbowSprite;
    public Sprite crossSprite;
    public Image backgroundImage; // Assign in Inspector


    // Connection booleans
    public bool connectsTop;
    public bool connectsRight;
    public bool connectsBottom;
    public bool connectsLeft;


    public void SetRandomType()
    {
        pipeType = (PipeType)Random.Range(0, 3);
        ApplyType();

        // Random 90° rotation
        float rotation = 90f * Random.Range(0, 4);
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);

        UpdateConnections(); // MUST be after rotation!
    }



    public void HighlightPath(bool on)
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = on ? new Color(0.3f, 1f, 0.6f) : Color.white; // greenish if on
        }
    }


    // Called when the player clicks the tile
    public void OnPointerClick(PointerEventData eventData)
    {
        float currentZ = transform.eulerAngles.z;
        float newZ = Mathf.Round((currentZ - 90f) / 90f) * 90f; // rotate by -90°, snap to nearest 90
        transform.rotation = Quaternion.Euler(0f, 0f, newZ % 360f);
        UpdateConnections();
    }

    public void ApplyType()
    {
        switch (pipeType)
        {
            case PipeType.Straight:
                tileImage.sprite = straightSprite;
                break;
            case PipeType.Elbow:
                tileImage.sprite = elbowSprite;
                break;
            case PipeType.Cross:
                tileImage.sprite = crossSprite;
                break;
        }
    }

    // Determines which sides this tile connects to
    public void UpdateConnections()
    {
        connectsTop = connectsRight = connectsBottom = connectsLeft = false;

        float rotation = transform.eulerAngles.z % 360f; // Normalize angle

        switch (pipeType)
        {
            case PipeType.Straight:
                if (Mathf.Approximately(rotation, 0f) || Mathf.Approximately(rotation, 180f))
                {
                    connectsTop = true;
                    connectsBottom = true;
                }
                else
                {
                    connectsLeft = true;
                    connectsRight = true;
                }
                break;

            case PipeType.Elbow:
                if (Mathf.Approximately(rotation, 0f))
                {
                    connectsBottom = true;
                    connectsRight = true;
                }
                else if (Mathf.Approximately(rotation, 90f))
                {
                    connectsRight = true;
                    connectsTop = true;
                }
                else if (Mathf.Approximately(rotation, 180f))
                {
                    connectsTop = true;
                    connectsLeft = true;
                }
                else if (Mathf.Approximately(rotation, 270f))
                {
                    connectsLeft = true;
                    connectsBottom = true;
                }
                break;

            case PipeType.Cross:
                connectsTop = connectsRight = connectsBottom = connectsLeft = true;
                break;
        }
    }
}


