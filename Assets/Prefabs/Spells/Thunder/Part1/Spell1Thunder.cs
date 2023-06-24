using Assets._Scripts.Spells;
using UnityEngine;

public class Spell1Thunder : SpellProjectileBase
{
    [SerializeField]
    GameObject prefabPart2;
    [SerializeField]
    GameObject prefabPart3;
    [SerializeField]
    float interval = 0.3f;
    [SerializeField]
    int counter = 4;

    protected void Awake()
    {
        MyAwake();
        InvokeRepeating("SpawnThunder", 0.1f, interval);
    }

    private void SpawnThunder()
    {
        counter--;

        if (counter < 0) 
        {
            CancelInvoke("SpawnThunder");
            Destroy(gameObject);
            return;
        }

        GameObject newPrefab2 = Instantiate(prefabPart2, transform.position, Quaternion.identity);
        float halfHeight = newPrefab2.GetComponent<Renderer>().bounds.extents.y;
        newPrefab2.transform.position += new Vector3(0f, halfHeight, 0f);
        GameObject newPrefab3 = Instantiate(prefabPart3, transform.position, transform.rotation);
    }
}