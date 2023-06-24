using UnityEngine;

public class SpellMagicSword : SpellBase
{
    GameObject spellCore; // Reference to the spellCore object
    [SerializeField]
    float rotationSpeed = 7f;
    float constRotationSpeed;
    [SerializeField]
    float rotationImpact = -65f;
    [SerializeField]
    float speedTransitio = 0.3f;

    protected void Awake()
    {
        SetSpellStats();
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
        Invoke("TimeOut", destroyTime); // Destroy the spellCore after 5 seconds
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
        if (collision.gameObject.TryGetComponent(out AttackHandler unit))
        {
            unit.DAMAGE(DMG, conditions);
            Destroy(spellCore); // Destroy the spellCore when collided with an object
        }
    }

    void TimeOut()
    {
        Destroy(spellCore); // Destroy the spellCore after a timeout
    }
}