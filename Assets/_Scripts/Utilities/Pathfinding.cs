using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Pathfinding : MonoBehaviour
{
    float scale = 1.6f;
    float dotSize = 0.2f;
    Vector2 offset = new Vector2(0f, 0f);
    Vector2[,] mapVector;
    List<Vector2Int> path;
    List<Vector2> pathVectors;

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
        path = new List<Vector2Int>();
        GenerateVectorMap();
    }

    private void OnDrawGizmos()
    {
        if (Application.IsPlaying(this))
        {
            GeneratePath(new Vector2(0, 0), (Vector2)GameManager.Player.transform.position);
            //Gizmos.color = Color.red;
            //Vector2Int pos = GetStandingTile(new Vector2(35, 18));
            //Gizmos.DrawSphere(mapVector[pos.x, pos.y], dotSize);
            Gizmos.color = Color.blue;

            foreach (Vector2Int v in path)
                Gizmos.DrawSphere(mapVector[v.x, v.y], dotSize);
        }
    }
    public void GeneratePath(Vector2 _origin, Vector2 _destination)
    {
        path.Clear();
        Vector2Int origin = GetStandingTile(_origin);
        Vector2Int dest = GetStandingTile(_destination);
        Vector2Int point = origin;
        float distance = Vector2Int.Distance(point, dest);
        path.Add(point);
        int i = 0;
        while (dest.x != point.x || dest.y != point.y)
        {
            float tempDist = Vector2Int.Distance(new Vector2Int(point.x + 1, point.y), dest);
            Vector2Int tempPoint = Vector2Int.zero;
            if (tempDist < distance) // up
            {
                tempPoint = new Vector2Int(point.x + 1, point.y);
                distance = tempDist;
            }
            tempDist = Vector2Int.Distance(new Vector2Int(point.x - 1, point.y), dest);
            if (tempDist < distance) // down
            {
                tempPoint = new Vector2Int(point.x - 1, point.y);
                distance = tempDist;

            }
            tempDist = Vector2Int.Distance(new Vector2Int(point.x + 1, point.y + 1), dest);
            if (tempDist < distance) //up right
            {
                tempPoint = new Vector2Int(point.x + 1, point.y + 1);
                distance = tempDist;

            }
            tempDist = Vector2Int.Distance(new Vector2Int(point.x - 1, point.y + 1), dest);
            if (tempDist < distance) //down right
            {
                tempPoint = new Vector2Int(point.x - 1, point.y + 1);
                distance = tempDist;

            }
            tempDist = Vector2Int.Distance(new Vector2Int(point.x - 1, point.y - 1), dest);
            if (tempDist < distance) //down left
            {
                tempPoint = new Vector2Int(point.x - 1, point.y - 1);
                distance = tempDist;

            }
            tempDist = Vector2Int.Distance(new Vector2Int(point.x + 1, point.y - 1), dest);
            if (tempDist < distance) //up left
            {
                tempPoint = new Vector2Int(point.x + 1, point.y - 1);
                distance = tempDist;

            }
            tempDist = Vector2Int.Distance(new Vector2Int(point.x, point.y - 1), dest);
            if (tempDist < distance) //left
            {
                tempPoint = new Vector2Int(point.x, point.y - 1);
                distance = tempDist;

            }
            tempDist = Vector2Int.Distance(new Vector2Int(point.x, point.y + 1), dest);
            if (tempDist < distance) //right
            {
                tempPoint = new Vector2Int(point.x, point.y + 1);
                distance = tempDist;
            }
            point = tempPoint;
            path.Add(point);
            i++;
        }
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