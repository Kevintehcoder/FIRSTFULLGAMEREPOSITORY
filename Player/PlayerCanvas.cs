using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{

    [SerializeField] private Transform target; 
    [SerializeField] private Vector3 offset = new Vector3(0, 0, 0);

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }


    void LateUpdate()
    {
        if (target == null) return;


        transform.position = target.position + offset;

    }
}
