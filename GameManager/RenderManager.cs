using System.Collections.Generic;
using UnityEngine;

public class RenderManager : MonoBehaviour
{
    static List<GameObject> allObjects = new List<GameObject>();

    [SerializeField] float renderDistance;
    float sqrRenderDistance;
    Camera cam;
    void Start()
    {
        cam = Camera.main;

        InvokeRepeating("RenderObjects", 0f, 0.03f);
        sqrRenderDistance = renderDistance * renderDistance;
    }

    private void OnDrawGizmos()
    {
        if (cam != null)
        {
            Gizmos.DrawWireSphere(cam.transform.position, renderDistance);
        }
    }
    void Update()
    {
        
    }

    public static void AddObject(GameObject obj)
    {
        allObjects.Add(obj);
    }


    public static void RemoveObject(GameObject obj)
    {
        allObjects.Remove(obj);
    }


    public void RenderObjects()
    {
        Vector2 camPos = cam.transform.position;

        foreach (GameObject obj in allObjects)
        {
            if (obj == null)
            {
                allObjects.Remove(obj);
                continue;
            }
            float sqrDist = (camPos - (Vector2)obj.transform.position).sqrMagnitude;

            if (sqrDist < sqrRenderDistance)
            {
                obj.SetActive(true);
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }
}
