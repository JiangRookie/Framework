using Framework;
using UnityEngine;

public class PoolKitExample : MonoBehaviour
{
    public GameObject Cube;
    ObjectPoolExample m_ObjectPoolExample;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PoolManager.Instance.Get<Cube>(Cube);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            m_ObjectPoolExample = PoolManager.Instance.Get<ObjectPoolExample>();
            m_ObjectPoolExample.Init();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            m_ObjectPoolExample.Recycle();
        }
    }
}