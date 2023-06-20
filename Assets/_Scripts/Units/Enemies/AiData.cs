using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data for context steering
/// </summary>
public class AiData : MonoBehaviour
{
    /// <summary>
    /// List of targets to follow
    /// </summary>
    public List<Transform> targets = null;
    /// <summary>
    /// List of obstacles to avoid
    /// </summary>
    public Collider2D[] obstacles = null;
    /// <summary>
    /// Currently target to follow
    /// </summary>
    public Transform currentTarget;

    /// <summary>
    /// Available directions for context
    /// </summary>
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
    protected int GetTargetsCount() => targets == null ? 0 : targets.Count;
}
