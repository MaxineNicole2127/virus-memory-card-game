using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCardScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer rend;
    [SerializeField] private Sprite frontImage;
    private Sprite backImage;

    [SerializeField] private float flipSpeed;
    private bool isCovered, canBePressed;

    [SerializeField] private GameControllerScript controller;

    // handles id of the image
    private int _spriteId;

    public int spriteId
    {
        get { return _spriteId; }
    }

    // START FUNCTION

    private void Start()
    {
        canBePressed = true;
        isCovered = true;
        backImage = rend.sprite;
    }

    // when card is pressed
    private void OnMouseDown()
    {
        if (isCovered && canBePressed && controller.canOpen)
        {
            StartCoroutine(FlipCard(true));
            controller.imageOpened(this);
        }
    }

    // flips the card
    private IEnumerator FlipCard(bool toOpen)
    {
        canBePressed = false;

        if (toOpen) // opens card
        {
            for (float i = 0f; i < 180f; i += flipSpeed)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    rend.sprite = frontImage;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }

        else // closes card
        {
            for (float i = 180f; i > 0; i -= flipSpeed)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    rend.sprite = backImage;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }

        canBePressed = true;
        isCovered = !isCovered;
    }


    // assigns the content of the card
    public void ChangeImage(int id, Sprite image)
    {
        _spriteId = id;
        frontImage = image;
    }

    //private void VirusContent()
    //{
        
    //}

    // close card
    public void CloseCard()
    {
        StartCoroutine(FlipCard(false));
    }
}
