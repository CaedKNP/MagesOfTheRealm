using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionUI : MonoBehaviour
{
    //GameObject Bar;
    RectTransform rectTransform;
    float initialWidth;
    int numberOfImages = 9; // Change this according to your requirements
    float rectWidth = 40;
    float finalWidth;
    public List<Sprite> Sprites;
    static int _conditionCountMax = 9;// later change to total condition count so that it works with more?????
    [SerializeField]
    public int _currentConditionCount = 0;
    void AddConditionSprite(int conditionID)
    {
        if (_currentConditionCount + 1 < _conditionCountMax)
        {
            Debug.Log("condition:" + conditionID + " added");
            _currentConditionCount++;
            SetRectWidth(_currentConditionCount);
        }

    }
    void RemoveConditionSprite(int conditionID)
    {
        if (_currentConditionCount - 1 != 0)
        {
            Debug.Log("condition:" + conditionID + " Removed");
            _currentConditionCount--;
            SetRectWidth(_currentConditionCount);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        //  rectWidth = rectTransform.rect.width;
        SetRectWidth(_currentConditionCount);

    }

    // Update is called once per frame
    void Update()
    {
        SetRectWidth(_currentConditionCount);//delete when conditions are changed dynamically 
    }

    void SetRectWidth(int currentCount)
    {
        finalWidth = rectWidth * currentCount;
        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.x = finalWidth;
        rectTransform.sizeDelta = sizeDelta;

    }
}
