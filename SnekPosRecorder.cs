using System.Collections.Generic;
using UnityEngine;

public class SnekPosRecorder : MonoBehaviour
{
    public float pointSpacing = 3.5f;
    public List<Vector2> pathPoints = new List<Vector2>();
    public int maxPoints = 500;

    Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (pathPoints.Count == 0 || Vector2.Distance(pathPoints[0], rb2d.position) > pointSpacing)
        {
            pathPoints.Insert(0, rb2d.position);
            if (pathPoints.Count > maxPoints)
                pathPoints.RemoveAt(pathPoints.Count - 1);
        }
    }
}
