using System;
using System.Collections.Generic;
using UnityEngine;

public class SpellDash : MonoBehaviour
{
    GameObject player;
    StaffRotation staffrotator;
    Camera mainCam;
    Vector2 mousePos;
    public float maxDistance = 20;

    protected void Awake()
    {
        player = GameManager.Player;
        mainCam = Camera.main;
        transform.rotation = Quaternion.identity; // Ustawienie braku rotacji
        SpawnInPlace();
    }

    void SpawnInPlace()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;
    }

    void Update()
    {
        CheckIfTooFar();
    }

    private void CheckIfTooFar()
    {
        // Sprawdź odległość między pozycją kursora a pozycją gracza
        float distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

        // Jeśli odległość jest większa niż maksymalna odległość, zbliż prefab do gracza
        if (distanceToPlayer > maxDistance)
        {
            Vector2 direction = mousePos - (Vector2)player.transform.position;
            direction = direction.normalized;
            transform.position = (Vector2)player.transform.position + direction * maxDistance;
        }

        //transform.rotation = Quaternion.identity; // Ustawienie braku rotacji
    }
}
