using UnityEngine;

public class TargetPlayer : MonoBehaviour
{
    [SerializeField] MiscLogicManager miscLogic;
    [SerializeField] public Transform Terminalpoint;

    public Transform target;

    public float startPos;

    public bool AllowedtoFollow = true;

    public bool isPresetRotation = false;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        if (isPresetRotation == false)
        {
            Vector2 dir = Terminalpoint.position - transform.position;
            startPos = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }

    }


    void Update()
    {
        if (AllowedtoFollow == true)
        {
            transform.rotation = miscLogic.RelativeFollowFocus(transform.position, target.position, startPos);
        }
    }
}
