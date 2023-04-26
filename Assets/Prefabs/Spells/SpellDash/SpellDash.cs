using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDash : SpellBase
{
    GameObject player;

    protected void Awake()
    {
        SetSpeedDestroyTime(3.5f, 0.3f); // Nowe wartosci dla speed i destroyTime
        base.MyAwake();
        player = GameManager.Player;
    }

    void Update()
    {
        Vector3 direction = transform.position - player.transform.position;
        player.transform.position += direction.normalized * Time.deltaTime * 5f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Destroy(gameObject);
        }
    }
}
