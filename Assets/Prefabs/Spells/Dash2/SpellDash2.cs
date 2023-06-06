using UnityEngine;

public class SpellDash2 : SpellBase
{
    Vector2 dir;
    GameObject player;
    float time;

    private void Awake()
    {
        SetSpellStats();
        player = GameManager.Player;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = player.transform.position;
        dir = mousePosition - (Vector2)transform.position;
        time = Time.time;
    }
    // Start is called before the first frame update
    void FixedUpdate()
    {
        if (!player.GetComponent<HeroUnitBase>().TryMove(dir, 4))
        {
            player.GetComponent<Rigidbody2D>().MovePosition((Vector2)player.transform.position + 4 * Time.fixedDeltaTime * -dir);
            Destroy(this);
        }
        if (Time.time > time + 0.2f)
            Destroy(this);
    }
}