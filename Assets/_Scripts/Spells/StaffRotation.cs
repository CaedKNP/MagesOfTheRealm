using UnityEngine;

public class StaffRotation : MonoBehaviour
{
    public GameObject pfSpell;
    public Transform spellTransform;
    public bool canFire = true;
    public float timeBeetwenCasting;

    Camera mainCam;
    Vector2 mousePos;
    Vector2 rotation;

    void Start()
    {
        //mainCam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
        mainCam = Camera.main;
    }

    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        rotation = mousePos - (Vector2)transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

}