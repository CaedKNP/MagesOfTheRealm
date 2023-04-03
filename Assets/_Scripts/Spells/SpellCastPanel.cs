using UnityEngine;

public class SpellCastPanel : MonoBehaviour
{
    public GameObject[] spells; // tablica czarów
    
    StaffRotation spellRotator; // referencja do rotatora
    GameObject player; // referencja do obiektu gracza

    void Start()
    {
        Invoke("FindSpellRotator", 0.5f);
        FindSpellRotator();
    }

    void FindSpellRotator()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //player = GameManager.Player;

        spellRotator = player.GetComponentInChildren<StaffRotation>();

        if (spellRotator == null)
        {
            Debug.LogWarning("spellRotator is null!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CastSpell(0); // rzucanie pierwszego czaru
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CastSpell(1); // rzucanie drugiego czaru
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CastSpell(2); // rzucanie trzeciego czaru
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CastSpell(3); // rzucanie czwartego czaru
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CastSpell(4); // rzucanie czwartego czaru
        }
    }

    void CastSpell(int index)
    {
        Debug.Log("Casting spell " + index);

        if (spellRotator != null)
        {
            Instantiate(spells[index], this.transform.position, this.transform.rotation);
        }
        else
        {
            Debug.LogWarning("SpellRotator is not assigned!");
            FindSpellRotator();
            //za pierwszym razem zawsze jest null
            Instantiate(spells[index], spellRotator.spellTransform.position, spellRotator.transform.rotation);
        }
    }
}