using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : SpellBase
{
    public GameObject player;

    protected void Awake()
    {
        SetSpeedDestroyTime(1f, 3f); // Nowe wartości dla speed i destroyTime
        base.MyAwake();
    }

    void Update()
    {
        Vector3 direction = transform.position - player.transform.position;
        player.transform.position += direction.normalized * Time.deltaTime * 5f;
    }
}
