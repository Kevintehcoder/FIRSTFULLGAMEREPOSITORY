using UnityEngine;

public class TargetFocus : MonoBehaviour
{

    float currentFacingRotation;

    void Awake()
    {
        /*if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        */
    }


    public Quaternion FollowFocus(Transform targetTransform, float rotMin = -45, float rotMax = 45)
    {
        Vector2 dir = targetTransform.position - transform.position;
        float degrees = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;


        currentFacingRotation = transform.lossyScale.x < 0 ? 180f : 0f;
        float relativeAngle = Mathf.DeltaAngle(currentFacingRotation, degrees);


        relativeAngle = Mathf.Clamp(relativeAngle, rotMin, rotMax);
        return Quaternion.Euler(0, 0, relativeAngle);
    }
}
