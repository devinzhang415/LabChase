using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class displayObject : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private GameObject[] images;
    [SerializeField] private GameObject whiteImage;
    [SerializeField] private GameObject blackScreen;

    [Header("Relative Transform")]
    [SerializeField] private Transform XROrigin;
    [Header("Choose a magnitude of Movement")]
    [SerializeField] private int movementMagnitude = 1;
    [Header("Choose a magnitude of scale")]
    [SerializeField] private float scaleMagnitude = 1f;
    [SerializeField] private float flashSpeed = 0.01f;

    private Vector3 changeVector;
    private int imageIndex;
    private bool isFlashing;
    private bool flashInstanceTracker;
    private bool isOneEye;

    // Enum for flashing toggle states
    public enum FlashingToggle
    {
        NoToggle,
        FlashingOff,
        FlashingOn,
    }
 
    public FlashingToggle flashingToggle = FlashingToggle.NoToggle;
    

    void Start()
    {
        imageIndex = 0;
        isFlashing = false;
        isOneEye = false;

        // Initialize images
        for (int i = 1; i < images.Length; i++)
        {
            images[i].SetActive(false);
        }
        images[0].SetActive(true);
        whiteImage.SetActive(false);
        blackScreen.SetActive(false);
    }

    void Update()
    {

    }

    // Movement methods
    public void moveRight()
    {
        changeVector = XROrigin.right * .1f;
        gameObject.transform.position += movementMagnitude * changeVector;
    }

    public void moveRightJ()
    {
        changeVector = XROrigin.right * .1f;
        gameObject.transform.position += movementMagnitude * changeVector * 10;
    }

    public void moveLeft()
    {
        changeVector = XROrigin.right * -.1f;
        gameObject.transform.position += movementMagnitude * changeVector;
    }

    public void moveLeftJ()
    {
        changeVector = XROrigin.right * -.1f;
        gameObject.transform.position += movementMagnitude * changeVector * 10;
    }

    public void moveUp()
    {
        changeVector = XROrigin.up * .1f;
        gameObject.transform.position += movementMagnitude * changeVector;
    }

    public void moveDown()
    {
        changeVector = XROrigin.up * -.1f;
        gameObject.transform.position += movementMagnitude * changeVector;
    }

    public void scaleUp()
    {
        gameObject.transform.localScale *= scaleMagnitude;
    }

    public void scaleDown()
    {
        gameObject.transform.localScale /= scaleMagnitude;
    }

    // Method to toggle flashing
    public void toggleFlashing()
    {
        if (!isFlashing)
        {
            images[imageIndex].SetActive(false);
            whiteImage.SetActive(true);
            isFlashing = true;
            flashingToggle = FlashingToggle.FlashingOn;
            StartCoroutine(flash());

        }
        else
        {
            images[imageIndex].SetActive(true);
            whiteImage.SetActive(false);
            isFlashing = false;
            flashingToggle = FlashingToggle.FlashingOff;
            StopCoroutine(flash());

        }
    }

    // Flashing effect
    private IEnumerator flash()
    {
        //toggles white every x seconds
        while (isFlashing)
        {
            yield return new WaitForSeconds(flashSpeed);
            whiteImage.SetActive(!whiteImage.activeSelf);
        }
        // on shutdown ensures that image returns to blank
        whiteImage.SetActive(false);
    }

    // Methods to cycle through images
    public void nextImage()
    {
        if (imageIndex + 1 >= images.Length)
        {
            Debug.Log("No more images");
        }
        else
        {
            images[imageIndex].SetActive(false);
            imageIndex++;
            images[imageIndex].SetActive(true);
        }
    }

    public void previousImage()
    {
        if (imageIndex - 1 < 0)
        {
            Debug.Log("No more images");
        }
        else
        {
            images[imageIndex].SetActive(false);
            imageIndex--;
            images[imageIndex].SetActive(true);
        }
    }

    public void toggleOneEye()
    {
        isOneEye = !isOneEye;
        blackScreen.SetActive(isOneEye);
    }
}
