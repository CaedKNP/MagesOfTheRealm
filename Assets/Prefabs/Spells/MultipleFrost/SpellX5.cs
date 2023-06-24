using UnityEngine;

public class SpellX5 : SpellBase
{
    [SerializeField]
    GameObject prefab;
    private void Awake()
    {
        Spawn();
        Destroy(this.gameObject);
    }

    void Spawn()
    {
        transform.Rotate(0f, 0f, -40f);
        Instantiate(prefab, transform.position, transform.rotation);
        transform.Rotate(0f, 0f, 20f);
        Instantiate(prefab, transform.position, transform.rotation);
        transform.Rotate(0f, 0f, 20f);
        Instantiate(prefab, transform.position, transform.rotation);
        transform.Rotate(0f, 0f, 20f);
        Instantiate(prefab, transform.position, transform.rotation);
        transform.Rotate(0f, 0f, 20f);
        Instantiate(prefab, transform.position, transform.rotation);
    }
}