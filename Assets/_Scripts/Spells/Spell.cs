using UnityEngine;

[CreateAssetMenu(fileName = "Spell")]
public class Spell : ScriptableObject
{
    public GameObject Prefab; //MonoBehaviour
    public int ID;
    public float cooldown;
    public SpellSlot spellSlot;

    // Used in menus
    public string Name;
    public string Description;
}