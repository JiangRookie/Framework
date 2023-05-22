using Framework;
using UnityEngine;

namespace Example
{
    public class PoolKitExample : MonoBehaviour
    {
        public GameObject Cube;
        ObjectPoolExample m_ObjectPoolExample;

        void Update()
        {
            // if (Input.GetKeyDown(KeyCode.A))
            // {
            //     PoolManager.Instance.Get<Cube>(Cube);
            // }
            // if (Input.GetKeyDown(KeyCode.S))
            // {
            //     m_ObjectPoolExample = PoolManager.Instance.Get<ObjectPoolExample>();
            //     m_ObjectPoolExample.Init();
            // }
            // if (Input.GetKeyDown(KeyCode.D))
            // {
            //     m_ObjectPoolExample.Recycle();
            // }

            if (Input.GetKeyDown(KeyCode.A))
            {
                PoolManager.Instance.ClearGameObject(Cube);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                GameObject Cube1 = PoolManager.Instance.Get(Cube);
                GameObject Cube2 = PoolManager.Instance.Get(Cube);
                GameObject Cube3 = PoolManager.Instance.Get(Cube);
                PoolManager.Instance.Recycle(Cube1);
                PoolManager.Instance.Recycle(Cube2);
                PoolManager.Instance.Recycle(Cube3);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                PoolManager.Instance.Clear();
            }
        }
    }
}