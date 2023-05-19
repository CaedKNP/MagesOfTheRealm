using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell2Thunder : MonoBehaviour
{
    public float timeOut = 0.3f;
    private void Awake()
    {
        Invoke("TimeOut", timeOut);
    }

    void TimeOut()
    {
        Destroy(gameObject);
    }
}