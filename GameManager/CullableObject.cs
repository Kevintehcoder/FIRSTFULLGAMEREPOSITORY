using UnityEngine;

public class CullableObject : MonoBehaviour
{

    protected virtual void Start()
    {
        RenderManager.AddObject(gameObject);
    }

    public virtual void OnDestroy()
    {
        RenderManager.RemoveObject(gameObject);
    }

}
