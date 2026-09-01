using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddObjectInView(GameObject obj)
    {
        obj.SetActive(true);

    }

    void RemoveObject(GameObject obj)
    {
        obj.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AddObjectInView(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        RemoveObject(collision.gameObject);
    }
}
