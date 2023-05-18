using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Spell")]
public class Spell : ScriptableObject
{
    public Sprite image;
    public GameObject Prefab; //MonoBehaviour
    public bool CastFromHeroeNoStaff = false;
    public int ID;
    public float cooldown;
    public SpellSlot spellSlot;

    // Used in menus
    public string Name;
    public string Description;
}