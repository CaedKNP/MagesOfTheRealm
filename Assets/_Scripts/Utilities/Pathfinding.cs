using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    float scale = 0.16f;
    float dotSize = 0.02f;
    Vector2 offset = new Vector2(0.05f, -0.05f);
    Vector2[,] mapVector;

    private void GenerateVectorMap()
    {
        mapVector = new Vector2[GameManager.map.GetLength(0), GameManager.map.GetLength(0)];
        for (int i = 0; i < GameManager.map.GetLength(0); i++)
        {
            for (int j = 0; j < GameManager.map.GetLength(1); j++)
            {
                int value = GameManager.map[i, j];

                Vector2 dotPos = new Vector3(i * scale + offset.x, j * scale + offset.y);
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
            Gizmos.color = Color.red;
            Vector2Int pos = GetStandingTile(GameManager.Player.transform.position);
            //Gizmos.DrawSphere(mapVector[pos.Item1, pos.Item2], 0.02f);
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