using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMultipleFireball : MonoBehaviour
{
    public GameObject fireballPrefab;
    private void Awake()
    {
        Spawn();
        Destroy(this.gameObject);
    }

    void Spawn()
    {
        transform.Rotate(0f, 0f, -30f);
        Instantiate(fireballPrefab, transform.position, transform.rotation);
        transform.Rotate(0f, 0f, 30f);
        Instantiate(fireballPrefab, transform.position, transform.rotation);
        transform.Rotate(0f, 0f, 30f);
        Instantiate(fireballPrefab, transform.position, transform.rotation);
    }
}
