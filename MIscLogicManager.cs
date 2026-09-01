using UnityEngine;

[CreateAssetMenu(fileName = "MIscLogicManager", menuName = "Scriptable Objects/MIscLogicManager")]
public class MiscLogicManager : ScriptableObject
{
    public Quaternion AbsoluteFollowFocus(Vector2 StartPos, Vector2 targetTransform, float currentFacingRotation = 0, float rotMin = -180, float rotMax = 180)
    {
        Quaternion relativeRotate = RelativeFollowFocus(StartPos, targetTransform, currentFacingRotation, rotMin, rotMax);
        Quaternion absoluteRotation = Quaternion.Euler(0, 0, currentFacingRotation) * relativeRotate;

        return absoluteRotation;
    }

    public Quaternion RelativeFollowFocus(Vector2 StartPos, Vector2 targetTransform, float currentFacingRotation = 0, float rotMin = -180, float rotMax = 180)
    {
        Vector2 dir = targetTransform - StartPos;
        float degrees = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float relativeAngle = Mathf.DeltaAngle(currentFacingRotation, degrees);

        relativeAngle = Mathf.Clamp(relativeAngle, rotMin, rotMax);
        return Quaternion.Euler(0, 0, relativeAngle);
    }
}
