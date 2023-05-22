using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework
{
    public class PoolManager : ManagerBase<PoolManager>
    {
        #region 内部类

        public class GameObjectPoolData
        {
            public readonly Queue<GameObject> QueuePool; // 对象池队列，存储回收的游戏对象
            public GameObject SubPoolRootNode => m_SubPoolRootNode;
            readonly GameObject m_SubPoolRootNode; // 子池根节点，用于组织回收的游戏对象

            public GameObjectPoolData(GameObject gameObj, GameObject PoolRootNode)
            {
                m_SubPoolRootNode = new GameObject(gameObj.name); // 创建子池根节点，并以回收的游戏对象名字命名
                m_SubPoolRootNode.Parent(PoolRootNode);
                QueuePool = new Queue<GameObject>();
                Recycle(gameObj);
            }

            /// <summary>
            /// 回收对象到对象池
            /// </summary>
            /// <param name="gameObj">要回收的游戏对象</param>
            public void Recycle(GameObject gameObj)
            {
                QueuePool.Enqueue(gameObj);
                gameObj.Hide().Parent(m_SubPoolRootNode);
            }

            /// <summary>
            /// 从对象池获取游戏对象
            /// </summary>
            /// <param name="parent">父节点</param>
            /// <returns>获取到的游戏对象</returns>
            public GameObject Get(Transform parent = null)
            {
                GameObject gameObj = QueuePool.Dequeue();
                gameObj.Show().Parent(parent);
                if (parent == null)
                {
                    SceneManager.MoveGameObjectToScene(gameObj, SceneManager.GetActiveScene());
                }
                return gameObj;
            }
        }

        public class ObjectPoolData
        {
            public readonly Queue<object> QueuePool; // 对象池队列，用于存储回收的对象

            public ObjectPoolData(object obj)
            {
                QueuePool = new Queue<object>();
                Recycle(obj);
            }

            /// <summary>
            /// 回收对象到对象池
            /// </summary>
            /// <param name="obj">要回收的对象</param>
            public void Recycle(object obj)
            {
                QueuePool.Enqueue(obj);
            }

            /// <summary>
            /// 从对象池获取对象
            /// </summary>
            /// <returns>获取到的对象</returns>
            public object Get()
            {
                return QueuePool.Dequeue();
            }
        }

        #endregion

        [SerializeField] GameObject m_PoolRootNode; // 对象池的根节点
        public Dictionary<string, GameObjectPoolData> GameObjectPoolDict = new Dictionary<string, GameObjectPoolData>();
        public Dictionary<string, ObjectPoolData> ObjectPoolDict = new Dictionary<string, ObjectPoolData>();

        #region GameObject

        /// <summary>
        /// 回收对象到对象池
        /// </summary>
        /// <param name="gameObj">要回收的对象</param>
        public void Recycle(GameObject gameObj)
        {
            string gameObjName = gameObj.name;
            if (GameObjectPoolDict.TryGetValue(gameObjName, out var subPool))
            {
                subPool.Recycle(gameObj);
            }
            else
            {
                GameObjectPoolDict.Add(gameObjName, new GameObjectPoolData(gameObj, m_PoolRootNode));
            }
        }

        /// <summary>
        /// 获取对象上的组件
        /// </summary>
        /// <param name="prefab">对象的预制体</param>
        /// <param name="parent">父节点</param>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns>获取到的组件</returns>
        public T Get<T>(GameObject prefab, Transform parent = null) where T : Component
        {
            GameObject gameObj = Get(prefab, parent);
            return gameObj != null ? gameObj.GetComponent<T>() : null;
        }

        /// <summary>
        /// 从对象池中获取指定预制体的游戏对象，如果对象池中没有可用对象，则实例化一个新的游戏对象
        /// </summary>
        /// <param name="prefab">预制体</param>
        /// <param name="parent">父节点</param>
        /// <returns>获取到的游戏对象</returns>
        public GameObject Get(GameObject prefab, Transform parent = null)
        {
            GameObject gameObj;
            string prefabName = prefab.name;
            if (CheckSubPoolCache(prefabName))
            {
                gameObj = GameObjectPoolDict[prefabName].Get(parent);
            }
            else
            {
                gameObj = Instantiate(prefab, parent);
                gameObj.name = prefabName;
            }
            return gameObj;
        }

        /// <summary>
        /// 检查指定预制体名称的子对象池缓存是否有可用对象
        /// </summary>
        /// <param name="prefabName">预制体名称</param>
        /// <returns>子对象池中是否有可用对象</returns>
        bool CheckSubPoolCache(string prefabName)
        {
            return GameObjectPoolDict.TryGetValue(prefabName, out var subPool) && subPool.QueuePool.Count > 0;
        }

        #endregion

        #region Object

        /// <summary>
        /// 回收对象到对象池
        /// </summary>
        /// <param name="obj">要回收的对象</param>
        public void Recycle(object obj)
        {
            string typeName = obj.GetType().FullName;
            if (ObjectPoolDict.TryGetValue(typeName, out var subPool))
            {
                subPool.Recycle(obj);
            }
            else
            {
                ObjectPoolDict.TryAdd(typeName, new ObjectPoolData(obj));
            }
        }

        /// <summary>
        /// 从对象池中获取指定类型的对象，如果对象池中没有可用对象，则创建一个新对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>获取到的对象</returns>
        public T Get<T>() where T : class, new()
        {
            if (CheckSubPoolCache<T>(out var typeName) && ObjectPoolDict.TryGetValue(typeName, out var objectPoolData))
            {
                return (T)objectPoolData.Get();
            }
            return new T();
        }

        /// <summary>
        /// 检查指定类型的子对象池缓存是否有可用对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="typeName">返回对象类型的完全限定名</param>
        /// <returns>子对象池中是否有可用对象</returns>
        bool CheckSubPoolCache<T>(out string typeName)
        {
            typeName = typeof(T).FullName;
            return !string.IsNullOrEmpty(typeName)
             && ObjectPoolDict.TryGetValue(typeName, out var subPool)
             && subPool.QueuePool.Count > 0;
        }

        #endregion

        public void ClearGameObjPool() => GameObjectPoolDict.Clear();
        public void ClearObjPool() => ObjectPoolDict.Clear();
    }
}