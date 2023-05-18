using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;
    public float smoothing;

    public Vector2 minPos;
    public Vector2 maxPos;

    void FixedUpdate()
    {
        if (target == null)
            return;
        if(transform.position != target.position)
        {
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

            targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);
            targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);

            transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
        }
    }
}