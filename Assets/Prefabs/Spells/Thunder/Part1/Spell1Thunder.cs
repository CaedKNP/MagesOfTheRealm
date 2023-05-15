using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spell1Thunder : SpellBase
{
    public GameObject prefabPart2;
    public GameObject prefabPart3;
    public float interval = 0.25f;
    public float deley = 0.3f;
    public int counter = 5;
    protected void Awake()
    {
        SetSpeedDestroyTime(13f, 100f); // Nowe wartości dla speed i destroyTime
        base.MyAwake();
        InvokeRepeating("SpawnThunder2", deley, interval);
    }

    private void SpawnThunder2()
    {
        counter--;
        if (counter == 0)
        {
            CancelInvoke("SpawnThunder2");
            Destroy(gameObject);
        }

        //Quaternion spawnRotation = Quaternion.Euler(0f, 0f, 0f);
        GameObject newPrefab2 = Instantiate(prefabPart2, transform.position, Quaternion.identity);

        // Przesunięcie pozycji prefabu wzdłuż osi Y o połowę wysokości prefaba
        float halfHeight = newPrefab2.GetComponent<Renderer>().bounds.extents.y;
        newPrefab2.transform.position += new Vector3(0f, halfHeight, 0f);

        GameObject newPrefab3 = Instantiate(prefabPart3, transform.position, transform.rotation);
    }


}