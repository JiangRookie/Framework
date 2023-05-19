using UnityEngine;

namespace Framework
{
    public interface IManager
    {
        public void Init();
    }

    public abstract class ManagerBase<T> : MonoBehaviour, IManager where T : ManagerBase<T>
    {
        public static T Instance;

        public virtual void Init()
        {
            Instance = this as T;
        }
    }
}