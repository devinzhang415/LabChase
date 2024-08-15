using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displayObject : MonoBehaviour
{
    [SerializeField] 
    private GameObject whiteImage;
    [Header("Choose a magnitute of Movement")]
    [SerializeField] private int movementMagnitute = 1;
    [Header("Choose a magnitute of scale")]
    [SerializeField] private float scaleMagnitute = 1f;

    private Vector3 changeVector;
    private bool isFlashing;


    // Start is called before the first frame update
    void Start()
    {
        isFlashing = true;
        whiteImage.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        flash();
    }

    public void moveRight()
    {
        changeVector = new Vector3(.1f, 0, 0);
        gameObject.transform.position += movementMagnitute * changeVector;
    }
    public void moveRightJ()
    {
        changeVector = new Vector3(.1f, 0, 0);
        gameObject.transform.position += movementMagnitute * changeVector * 10;
    }
    public void moveLeft()
    {
        changeVector = new Vector3(-.1f, 0, 0);
        gameObject.transform.position += movementMagnitute * changeVector;
    }
    public void moveLeftJ()
    {
        changeVector = new Vector3(-.1f, 0, 0);
        gameObject.transform.position += movementMagnitute * changeVector * 10;
    }
    public void moveUp()
    {
        changeVector = new Vector3(0, .1f, 0);
        gameObject.transform.position += movementMagnitute * changeVector;
    }
    public void moveDown()
    {
        changeVector = new Vector3(0, -.1f, 0);
        gameObject.transform.position += movementMagnitute * changeVector;
    }
    public void scaleUp()
    {
        gameObject.transform.localScale *= scaleMagnitute;
    }
    public void scaleDown()
    {
        gameObject.transform.localScale /= scaleMagnitute;
    }

    public void toggleFlashing()
    {
        if (!isFlashing)
        {
            whiteImage.SetActive(true);
            isFlashing = true;
        }
        else
        {
            whiteImage.SetActive(false);
            isFlashing = false;
        }
    }

    private void flash()
    {
        if (isFlashing)
        {
            whiteImage.SetActive(!whiteImage.activeSelf);
        }
    }
}
