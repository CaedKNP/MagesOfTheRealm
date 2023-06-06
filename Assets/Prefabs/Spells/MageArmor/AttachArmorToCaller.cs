using UnityEngine;

public class AttachArmorToCaller : MonoBehaviour
{
    public GameObject ArmorPrefab;
    public Vector3 armorOffset = new Vector3(0f, 0f, 1f); // Przesunięcie pancerza względem gracza

    void Start()
    {
        if (ArmorPrefab != null)
        {
            AttachArmorToPlayer();
        }
        else
        {
            Debug.LogError("No Armor prefab assigned!");
        }
        Destroy(gameObject);
    }

    void AttachArmorToPlayer()
    {
        GameObject player = GameManager.Player;

        if (player != null)
        {
            Transform firstChild = player.transform.GetChild(0);

            if (firstChild != null)
            {
                GameObject armorInstance = Instantiate(ArmorPrefab, firstChild);
                armorInstance.transform.position = firstChild.position + armorOffset;
                armorInstance.transform.rotation = Quaternion.identity;
            }
            else
            {
                Debug.LogError("Player does not have any child objects!");
            }
        }
        else
        {
            Debug.LogError("Player object not found!");
        }
    }
}
