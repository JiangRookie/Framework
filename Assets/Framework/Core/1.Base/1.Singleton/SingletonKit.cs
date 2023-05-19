using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 普通类的单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : Singleton<T>, new()
    {
        static T s_Instance;
        static readonly object s_LockObject = new();

        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (s_LockObject)
                    {
                        if (s_Instance == null)
                        {
                            s_Instance = new T();
                        }
                    }
                }
                return s_Instance;
            }
        }
    }

    /// <summary>
    /// MonoBehaviour 类的单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T Instance;

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
        }
    }

    /// <summary>
    /// 持久化 MonoBehaviour类的单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PersistentMonoSingleton<T> : MonoSingleton<T> where T : PersistentMonoSingleton<T>
    {
        protected override void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            base.Awake();
            DontDestroyOnLoad(this);
        }
    }
}