using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionUI : MonoBehaviour
{
    public Sprite m_Sprite;
    int conditionCountMax =9;// later change to total condition count so that it works with more?????
    void AddConditionSprite(int slot, int conditionID)
    {
        if(slot+1<conditionCountMax)
        {
            Debug.Log("condition:" + conditionID + " added");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
