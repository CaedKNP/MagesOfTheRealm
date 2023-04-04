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
    float timer;

    void Start()
    {
        mainCam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
    }

    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        rotation = mousePos - (Vector2)transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBeetwenCasting)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && canFire)
        {
            canFire = false;
            CastSpell();
        }
    }

    void CastSpell()
    {
        Instantiate(pfSpell, spellTransform.position, transform.rotation);
    }
}