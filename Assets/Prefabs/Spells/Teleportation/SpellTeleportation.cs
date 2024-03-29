﻿using System;
using System.Collections;
using UnityEngine;

public class SpellTeleportation : MonoBehaviour
{
    GameObject player;
    StaffRotation staffrotator;
    Camera mainCam;
    Vector2 mousePos;
    public float maxDistance = 20f; // Maksymalna odległość od gracza
    public float chekingSafePlaceSpeed = 10f;
    bool isMoving = true; // Czy prefab ma się poruszać
    public Collider2D bigPrefabCollider; // Referencja do Collidera większego Prefabu
    public float moveSpeed = 5f;
    private bool isOnWater = false;


    private void Awake()
    {
        player = GameManager.Player;
        mainCam = Camera.main;
        SpawnInPlace();
    }

    private void Update()
    {
        // Poruszanie się tylko po wodzie i zbliżanie do gracza
        if (isMoving && isOnWater)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            // Jeśli prefabrykat wejdzie na wodę
            isOnWater = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            // Jeśli prefabrykat opuści wodę
            isOnWater = false;
            // Zatrzymanie prefabrykatu
            StopMovement();
        }
    }

    private void StopMovement()
    {
        // Zatrzymanie ruchu prefabrykatu
        isMoving = false;
    }

    private bool CheckIfOnWater()
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position); // Sprawdź kolizję w punkcie pozycji Prefabu "dash"

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Water")) // Sprawdź, czy collider należy do obiektu o tagu "WaterTag"
            {
                return true;
            }
        }

        return false; // Prefab "dash" nie znajduje się na wodzie
    }

    void SpawnInPlace()
    {
        transform.rotation = Quaternion.identity; // Ustawienie braku rotacji

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // Oblicz odległość między pozycją gracza a pozycją kursora
        float distanceToPlayer = Vector2.Distance(player.transform.position, mousePos);

        // Jeśli odległość jest większa niż maksymalna odległość, zbliż prefab do gracza
        if (distanceToPlayer > maxDistance)
        {
            Vector2 direction = mousePos - (Vector2)player.transform.position;
            direction = direction.normalized;
            transform.position = (Vector2)player.transform.position + direction * maxDistance;
        }
        else
        {
            transform.position = mousePos;
        }
    }
}
