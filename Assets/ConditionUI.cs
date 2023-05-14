using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionUI : MonoBehaviour
{
    RectTransform _rectTransform;

    float _rectWidth = 40;
    float _finalWidth;
    int _conditionCountMax = 9;// later change to total condition count so that it works with more?????

    [SerializeField]
    int _currentConditionCount = 0;

    public List<Sprite> sprites;
    public List<Image> imageSlots;


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
           
                // Shift the sprites within the images
                for (int i = indexOfImgToBeRemoved + 1; i < imageSlots.Count; i++)
                {
                    SetImageSprite(imageSlots[i - 1], imageSlots[i].sprite);
                }

                // Clear the last sprite
                SetImageSprite(imageSlots[_currentConditionCount],null);

            }
            _currentConditionCount--;
            SetRectWidth(_currentConditionCount);
            Debug.Log("condition: removed");
        }
    }

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();

        //Uncomment for testing
        //should be: [Burn,DmgUp,Cooldown]
        //AddConditionSprite(3);
        //AddConditionSprite(0);
        //AddConditionSprite(5);
        //AddConditionSprite(6);
        //RemoveConditionSprite(3);
        //RemoveConditionSprite(5);
        //AddConditionSprite(7);
        //RemoveConditionSprite(6);
        //AddConditionSprite(8);
        //RemoveConditionSprite(7);
        //AddConditionSprite(7);

        SetRectWidth(_currentConditionCount);

    }

    void SetRectWidth(int currentCount)
    {
        _finalWidth = _rectWidth * currentCount;
        Vector2 sizeDelta = _rectTransform.sizeDelta;
        sizeDelta.x = _finalWidth;
        sizeDelta.y = 40;
        _rectTransform.sizeDelta = sizeDelta;

    }

    void SetImageSprite(Image img, Sprite sprite)
    {
        img.sprite = sprite;
    }

}
