using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Pathfinding : MonoBehaviour
{
    float scale = 1.6f;
    float dotSize = 0.02f;
    Vector2 offset = new Vector2(0f, 0f);
    Vector2[,] mapVector;

    // Update is called once per frame
    

    private void GenerateVectorMap()
    {

        mapVector = new Vector2[GameManager.map.GetLength(0), GameManager.map.GetLength(0)];
        for (int i = 0; i < GameManager.map.GetLength(0); i++)
        {
            for (int j = 0; j < GameManager.map.GetLength(1); j++)
            {
                int value = GameManager.map[i, j];

                // Draw the dot
                Vector2 dotPos = new Vector2(i * scale + offset.x, j * scale + offset.y);
                mapVector[i, j] = dotPos;
            }
        }

    }
    private void Start()
    {
        GenerateVectorMap();
    }

    private void OnDrawGizmos()
    {
        if (Application.IsPlaying(this))
        {
            Gizmos.color = Color.blue;
            //Vector2Int pos = GetStandingTile(GameManager.Player.transform.position);
            //Gizmos.DrawSphere(mapVector[pos.x, pos.x], 0.5f);
            foreach (Vector2 v in mapVector)
                Gizmos.DrawSphere(v, 0.5f);
        }
    }
    public void GeneratePath(Vector2 origin, Vector2 destination)
    {
        Vector2Int originMap = GetStandingTile(origin);
        Vector2Int destMap = GetStandingTile(destination);


    }

    public Vector2Int GetStandingTile(Vector2 target)
    {
        Vector2Int pos = new(0, 0);
        for (int i = 0; i < mapVector.GetLength(0); i++)
        {
            for (int j = 0; j < mapVector.GetLength(1); j++)
            {
                if (Vector2.Distance(target, mapVector[i, j]) < Vector2.Distance(target, mapVector[pos.x, pos.y]))
                    pos = new(i, j);
            }
        }

        return pos;
    }

    public Vector2Int GetStandingTile(Vector3 _target)
    {
        Vector2 target = (Vector2)_target;

        Vector2Int pos = new(0, 0);
        for (int i = 0; i < mapVector.GetLength(0); i++)
        {
            for (int j = 0; j < mapVector.GetLength(1); j++)
            {
                if (Vector2.Distance(target, mapVector[i, j]) < Vector2.Distance(target, mapVector[pos.x, pos.y]))
                    pos = new(i, j);
            }
        }

        return pos;
    }
}