using UnityEngine;

public class ManekinLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out HeroUnitBase unit))
        {
            unit.ChangeMage(this.name);
        }
    }
}