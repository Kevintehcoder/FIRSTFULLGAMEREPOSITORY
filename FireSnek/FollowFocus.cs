using UnityEngine;

public class FollowFocus : MonoBehaviour
{
    [SerializeField] Transform Focus;
    public static float Speed;

    public static float NormalresponseTime = 0.45f;
    public static float SpeedyresponseTime = 0.25f;

    public static float currentResponseTime = 0;

    float minDistance = 3.2f;

    [HideInInspector]public Rigidbody2D rb2d;

    Vector2 currentVelocity;
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        currentResponseTime = NormalresponseTime;
    }


    public void followFocus()
    {

        if (Vector2.Distance(rb2d.position, Focus.position) > minDistance)
        {
            Vector2 targetPos = Vector2.SmoothDamp(
                rb2d.position,
                Focus.position,
                ref currentVelocity,
                currentResponseTime,
                Speed,
                Time.fixedDeltaTime
            );
            rb2d.MovePosition(targetPos);
        }
        else 
        {
            rb2d.linearVelocity = Vector2.zero;
        }


        Vector2 dir = (Vector2)Focus.position - rb2d.position;

        float targetangle = 180 + Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        rb2d.MoveRotation(targetangle);

    }


}
