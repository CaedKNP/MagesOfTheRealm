using UnityEngine;

public class ManekinLogic : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out HeroUnitBase unit))
        {
            unit.ChangeMage(name);
        }
    }
}