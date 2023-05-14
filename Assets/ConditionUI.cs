using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionUI : MonoBehaviour
{
    //GameObject Bar;
    RectTransform rectTransform;
    float initialWidth;
    int numberOfImages = 9; // Change this according to your requirements
    float rectWidth = 40;
    float finalWidth;
    public List<Sprite> sprites;
    public Component parentContainer;
    public List<Image> imageSlots;
    int _conditionCountMax = 9;// later change to total condition count so that it works with more?????
    [SerializeField]
    int _currentConditionCount = 0;
    void AddConditionSprite(int condition)
    {
        if (_currentConditionCount + 1 < _conditionCountMax)
        {


            SetRectWidth(_currentConditionCount);
            SetImageSprite(imageSlots[_currentConditionCount], sprites[condition]);
            _currentConditionCount++;

            Debug.Log("condition: added");
        }

    }
    void RemoveConditionSprite(int condition)
    {
        if (_currentConditionCount - 1 >= 0)
        {
            Image imageWithSprite = this.imageSlots.Find(image => image.sprite == sprites[condition]);
            int indexOfImgToBeRemoved = this.imageSlots.IndexOf(imageWithSprite);

            if (indexOfImgToBeRemoved >= 0 && indexOfImgToBeRemoved < imageSlots.Count)

            {
                Sprite tempSprite = imageSlots[indexOfImgToBeRemoved].sprite;

                // Shift the sprites within the images
                for (int i = indexOfImgToBeRemoved + 1; i < imageSlots.Count; i++)
                {
                    imageSlots[i - 1].sprite = imageSlots[i].sprite;
                }

                // Set the sprite of the last image to null or any other desired sprite
                imageSlots[_currentConditionCount].sprite = null;

                // Set the sprite of the first image to the temporary sprite
                //imageSlots[indexOfImgToBeRemoved].sprite = tempSprite;
            }
            _currentConditionCount--;
            SetRectWidth(_currentConditionCount);
            Debug.Log("condition: removed");
        }
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        SetRectWidth(_currentConditionCount);

    }

    void SetRectWidth(int currentCount)
    {
        finalWidth = rectWidth * currentCount;
        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.x = finalWidth;
        sizeDelta.y = 40;
        rectTransform.sizeDelta = sizeDelta;

    }

    void SetImageSprite(Image img, Sprite sprite)
    {
        img.sprite = sprite;
    }

}
