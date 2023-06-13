using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiData : MonoBehaviour
{
    public List<Transform> targets = null;
    public Collider2D[] obstacles = null;
    public Transform currentTarget;

    public Vector2[] direction = new Vector2[8]
    {
        new Vector2(0, 1).normalized,
        new Vector2(1, 1).normalized,
        new Vector2(1, 0).normalized,
        new Vector2(1, -1).normalized,
        new Vector2(0, -1).normalized,
        new Vector2(-1, -1).normalized,
        new Vector2(-1, 0).normalized,
        new Vector2(-1, 1).normalized
    };
    public int GetTargetsCount() => targets == null ? 0 : targets.Count;
}
