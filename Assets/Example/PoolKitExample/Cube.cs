using Framework;
using UnityEngine;

public class Cube : MonoBehaviour
{
    void OnEnable()
    {
        Invoke(nameof(Destroy), 3f);
    }

    void Destroy()
    {
        PoolManager.Instance.Recycle(gameObject);
    }
}