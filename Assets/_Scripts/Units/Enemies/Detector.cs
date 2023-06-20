using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class for detecting stuff on map
/// </summary>
public class Detector : MonoBehaviour
{
    [SerializeField]
    private AiData aiData;

    /// <summary>
    /// Detects all obstacle around enemy
    /// </summary>
    public void DetectObstacles()
    {
        aiData.obstacles = Physics2D.OverlapCircleAll(transform.position, 2).ToList().Where(x => x.tag != "Player").ToArray();
    }
}
