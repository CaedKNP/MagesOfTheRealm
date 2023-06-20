using UnityEngine;

public class StaffRotation : MonoBehaviour
{
    [SerializeField]
    private GameObject pfSpell;
    [SerializeField]
    private Transform spellTransform;

    /// <summary> Transformation of the spell casting place for player</summary>
    public Transform WizandStaffFirePint;

    private Camera mainCam;

    [SerializeField] 
    private Vector2 mousePos;

    private Vector2 rotation;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        rotation = mousePos - (Vector2)transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

}