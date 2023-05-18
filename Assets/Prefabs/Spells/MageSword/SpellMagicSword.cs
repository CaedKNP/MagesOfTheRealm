using Assets._Scripts.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpellMagicSword : MonoBehaviour
{
    private GameObject spellCore; // Reference to the spellCore object
    public float rotationSpeed = 7f;
    float constRotationSpeed;
    public float rotationImpact = -65f;
    public float speedTransitio = 0.3f;

    protected void Awake()
    {
        // Find the spellCore object in the swordCore prefab
        spellCore = transform.parent?.gameObject;

        float initialRotation = spellCore.transform.rotation.eulerAngles.z;
        bool rotateLeft = initialRotation >= 90f && initialRotation <= 270f;
        bool rotateRight = !rotateLeft;

        if (rotateRight)
        {
            constRotationSpeed = rotationSpeed; // save speed
            rotationSpeed = -rotationSpeed; // Reverse rotation
            rotationImpact = -rotationImpact;
            Invoke("SpeedTransition", speedTransitio);
        }

        spellCore.transform.Rotate(Vector3.forward, rotationImpact); // Rotate the spellCore at the start by -65 degrees
        Invoke("TimeOut", 5f); // Destroy the spellCore after 5 seconds
    }

    private void FixedUpdate()
    {
        spellCore.transform.Rotate(Vector3.forward, rotationSpeed); // Rotate the spellCore continuously
    }

    private void SpeedTransition()
    {
        rotationSpeed = constRotationSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var conditions = new List<ConditionBase>
        {
            new ConditionBase() { Conditions = Conditions.ArmorDown, AffectOnTick = 0.2f, AffectTime = 2f }
        };

        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(5, conditions);
            Destroy(spellCore); // Destroy the spellCore when collided with an object
        }
    }

    void TimeOut()
    {
        Destroy(spellCore); // Destroy the spellCore after a timeout
    }
}
