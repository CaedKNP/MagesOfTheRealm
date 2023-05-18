using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spell1Thunder : SpellBase
{
    public GameObject prefabPart2;
    public GameObject prefabPart3;
    public float interval = 0.3f; // Zmieniono wartość interwału na 0.3f
    public int counter = 4; // Zmieniono liczbę powtórzeń na 4

    protected void Awake()
    {
        SetSpeedDestroyTime(13f, 100f); // Nowe wartości dla speed i destroyTime
        base.MyAwake();
        InvokeRepeating("SpawnThunder", 0.1f, interval); // Wywołanie metody SpawnThunder z interwałem 0.3 sekundy
    }

    private void SpawnThunder()
    {
        counter--;

        if (counter < 0) // Sprawdzenie, czy licznik jest mniejszy od zera
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
