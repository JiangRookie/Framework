using Framework;
using UnityEngine;

namespace Example
{
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

    public class ObjectPoolExample
    {
        public void Init() => Debug.Log("我产生了");

        public void Recycle()
        {
            PoolManager.Instance.Recycle(this);
        }
    }
}