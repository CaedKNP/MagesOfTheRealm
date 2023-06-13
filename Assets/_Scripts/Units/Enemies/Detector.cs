using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField]
    private AiData aiData;

    public void DetectObstacles()
    {
        aiData.obstacles = Physics2D.OverlapCircleAll(transform.position, 2).ToList().Where(x => x.tag != "Player").ToArray();
    }
}
