using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellX3 : SpellBase
{
    public GameObject prefab;
    private void Awake()
    {
        Spawn();
        Destroy(this.gameObject);
    }

    void Spawn()
    {
        transform.Rotate(0f, 0f, -30f);
        Instantiate(prefab, transform.position, transform.rotation).GetComponent<SpellBase>().caster = caster;
        transform.Rotate(0f, 0f, 30f);
        Instantiate(prefab, transform.position, transform.rotation).GetComponent<SpellBase>().caster = caster;
        transform.Rotate(0f, 0f, 30f);
        Instantiate(prefab, transform.position, transform.rotation).GetComponent<SpellBase>().caster = caster;
    }
}