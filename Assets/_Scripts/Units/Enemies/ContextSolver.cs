using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

/// <summary>
/// Solve for best direction and maange context steering logic
/// </summary>
public class ContextSolver : MonoBehaviour
{
    [SerializeField]
    private AiData aiData;

    private float[] wagesGood;
    private float[] wagesBad;
    private float[] wages;

    private void Awake()
    {
        wagesGood = new float[aiData.direction.Length];
        wagesBad = new float[aiData.direction.Length];
        wages = new float[aiData.direction.Length];
    }

    /// <summary>
    /// Solve context steering to choose best directions to move
    /// </summary>
    /// <returns>Direction that is best to move</returns>
    public Vector2 ChooseDirection()
    {
        Vector2 heading = Vector2.zero;

        if (aiData.currentTarget == null)
        {
            if(aiData.targets.Count == 0)
                return Vector2.zero;
            aiData.currentTarget = aiData.targets.First();
            aiData.targets.RemoveAt(0);
        }

        SolveObstacles();
        SolveTarget();

        for (int i = 0; i < 8; i++)
        {
            wages[i] = Mathf.Clamp01(wagesGood[i] - wagesBad[i]);
        }

        for (int i = 0; i < 8; i++)
        {
            heading += aiData.direction[i] * wages[i];
        }

        heading.Normalize();
        return heading;
    }

    private void SolveObstacles()
    {
        foreach (Collider2D obstacleCollider in aiData.obstacles)
        {
            Vector2 directionToObstacle
                = obstacleCollider.ClosestPoint(transform.position) - (Vector2)transform.position;
            float distanceToObstacle = directionToObstacle.magnitude;

            //calculate weight based on the distance Enemy<--->Obstacle
            float weight = distanceToObstacle;

            Vector2 directionToObstacleNormalized = directionToObstacle.normalized;

            //Add obstacle parameters to the danger array
            for (int i = 0; i < aiData.direction.Length; i++)
            {
                float result = Vector2.Dot(directionToObstacleNormalized, aiData.direction[i]);

                float valueToPutIn = result * weight;

                //override value only if it is higher than the current one stored in the danger array
                if (valueToPutIn > wagesBad[i])
                {
                    wagesBad[i] = valueToPutIn;
                }
            }
        }
    }

    private void SolveTarget()
    {

        for (int i = 0; i < 8; i++)
        {
            float result = Vector2.Dot(aiData.currentTarget.position - transform.position, aiData.direction[i]);
            if (result > 0)
            {
                wagesGood[i] = result;
            }
        }
    }
}
