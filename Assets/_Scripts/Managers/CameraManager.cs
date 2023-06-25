using UnityEngine;

/// <summary>
/// Controls the camera and its movement
/// </summary>
public class CameraManager : MonoBehaviour
{
    /// <summary>
    /// position of looking object (player)
    /// </summary>
    public Transform target;

    /// <summary>
    /// value of smoothing camera movement
    /// </summary>
    public float smoothing;

    /// <summary>
    /// limitation of camera movement
    /// </summary>
    public Vector2 minPos;
    public Vector2 maxPos;

    void FixedUpdate()
    {
        if (target == null)
            return;

        if (transform.position != target.position)
        {
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

            targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);
            targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);

            transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
        }
    }
}