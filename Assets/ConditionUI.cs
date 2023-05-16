using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionUI : MonoBehaviour
{
    public RectTransform _rectTransform;

    float _rectWidth = 40;
    float _finalWidth;

    [SerializeField]
    int _currentConditionCount = 0;

    public List<Sprite> sprites;
    public List<Image> imageSlots;


    public void AddConditionSprite(int condition)
    {
        Debug.Log("AddConditionSprite");
        
        SetImageSprite(imageSlots[_currentConditionCount], sprites[condition]);

        _currentConditionCount++;

        SetRectWidth(_currentConditionCount);

        Debug.Log("condition: added");

    }

    public void RemoveConditionSprite(int condition)
    {
        Debug.Log("delito");
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
                SetImageSprite(imageSlots[_currentConditionCount], null);

            }
            _currentConditionCount--;
            SetRectWidth(_currentConditionCount);
            Debug.Log("condition: removed");
        }
    }

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        Debug.Log("wrrr");
        //Uncomment for testing
        //should be: [Burn,DmgUp,Cooldown]

        //AddConditionSprite(0);
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
