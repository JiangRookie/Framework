using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework
{
    public class PoolManager : ManagerBase<PoolManager>
    {
        [SerializeField] GameObject m_PoolRootNode;
        public Dictionary<string, GameObjectPoolData> GameObjectPoolDict = new();

        public override void Init()
        {
            base.Init();
            Debug.Log("The PoolManager was successfully initialized!");
        }

        /// <summary>
        /// 获取对象身上的组件
        /// </summary>
        /// <param name="prefab"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(GameObject prefab) where T : Component
        {
            GameObject gameObj = Get(prefab);
            return gameObj != null ? gameObj.GetComponent<T>() : null;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public GameObject Get(GameObject prefab)
        {
            GameObject gameObj;
            string prefabName = prefab.name;
            if (CheckSubPoolCache(prefab))
            {
                gameObj = GameObjectPoolDict[prefabName].Get();
            }
            else
            {
                gameObj = Instantiate(prefab);
                gameObj.name = prefabName;
            }
            return gameObj;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="gameObj"></param>
        public void Recycle(GameObject gameObj)
        {
            string gameObjName = gameObj.name;
            if (GameObjectPoolDict.TryGetValue(gameObjName, out var gameObjectPoolData))
            {
                gameObjectPoolData.Recycle(gameObj);
            }
            else
            {
                GameObjectPoolDict.Add(gameObjName, new GameObjectPoolData(gameObj, m_PoolRootNode));
            }
        }

        /// <summary>
        /// 清除对象池字典
        /// </summary>
        public void Clear()
        {
            GameObjectPoolDict.Clear();
        }

        /// <summary>
        /// 检查子对象池缓存
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        bool CheckSubPoolCache(GameObject prefab)
        {
            string prefabName = prefab.name;
            return GameObjectPoolDict.ContainsKey(prefabName) && GameObjectPoolDict[prefabName].QueuePool.Count > 0;
        }

        public class GameObjectPoolData
        {
            public readonly Queue<GameObject> QueuePool;
            public readonly GameObject SubPoolRootNode;

            public GameObjectPoolData(GameObject gameObj, GameObject PoolRootNode)
            {
                SubPoolRootNode = new GameObject(gameObj.name);
                SubPoolRootNode.Parent(PoolRootNode);
                QueuePool = new Queue<GameObject>();
                Recycle(gameObj);
            }

            /// <summary>
            /// 回收对象
            /// </summary>
            /// <param name="gameObj"></param>
            public void Recycle(GameObject gameObj)
            {
                QueuePool.Enqueue(gameObj);
                gameObj.Parent(SubPoolRootNode).Hide();
            }

            /// <summary>
            /// 获取对象
            /// </summary>
            /// <returns></returns>
            public GameObject Get()
            {
                var gameObj = QueuePool.Dequeue();
                gameObj.Show().Parent(null);
                SceneManager.MoveGameObjectToScene(gameObj, SceneManager.GetActiveScene());
                return gameObj;
            }
        }
    }
}