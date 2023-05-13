using UnityEngine;

public class AttachSwordToCaller : MonoBehaviour
{
    public GameObject swordPrefab; // Sword prefab

    void Start()
    {
        if (swordPrefab != null)
        {
            AttachSwordToPlayer();
        }
        else
        {
            Debug.LogError("No sword prefab assigned!");
        }
        Destroy(gameObject);
    }

    void AttachSwordToPlayer()
    {
        GameObject player = GameManager.Player;

        if (player != null)
        {
            Transform staffRotator = player.transform.Find("StaffRotator");

            if (staffRotator != null)
            {
                GameObject swordInstance = Instantiate(swordPrefab, staffRotator);
                swordInstance.transform.localPosition = new Vector3(0f, 0f, 0f);
                swordInstance.SetActive(true); // Włącz renderowanie miecza
            }
            else
            {
                Debug.LogError("StaffRotator child object not found on the player!");
            }
        }
        else
        {
            Debug.LogError("Player object not found!");
        }
    }
}
