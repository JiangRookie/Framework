using System;
using System.Collections;
using System.Collections.Generic;
using Example;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework
{
    public class ResManager : ManagerBase<ResManager>
    {
        Dictionary<Type, bool> m_RequireCacheDict; // 需要缓存类型的字典

        public override void Init()
        {
            base.Init();
            m_RequireCacheDict = new Dictionary<Type, bool>();
            m_RequireCacheDict.Add(typeof(Cube), true); // Test
        }

        /// <summary>
        /// 检查类型是否在缓存字典中
        /// </summary>
        /// <param name="type">要检查的类型</param>
        /// <returns>是否存在于缓存字典中</returns>
        bool CheckCacheDict(Type type)
        {
            return m_RequireCacheDict.ContainsKey(type);
        }

        /// <summary>
        /// 根据类型加载对象
        /// </summary>
        /// <typeparam name="T">要加载的对象类型</typeparam>
        /// <returns>加载的对象实例</returns>
        public T Load<T>() where T : class, new()
        {
            if (CheckCacheDict(typeof(T)))
            {
                return PoolManager.Instance.Get<T>();
            }
            return new T();
        }

        /// <summary>
        /// 根据类型和路径加载对象
        /// </summary>
        /// <typeparam name="T">要加载的组件类型</typeparam>
        /// <param name="path">预制体的资源路径</param>
        /// <param name="parent">父对象的Transform（可选）</param>
        /// <returns>加载的组件实例</returns>
        public T Load<T>(string path, Transform parent = null) where T : Component
        {
            if (CheckCacheDict(typeof(T)))
            {
                return PoolManager.Instance.Get<T>(GetPrefab(path), parent);
            }
            return InstantiatePrefab(path, parent).GetComponent<T>();
        }

        /// <summary>
        /// 加载资源对象
        /// </summary>
        /// <typeparam name="T">要加载的资源类型</typeparam>
        /// <param name="path">资源的路径</param>
        /// <returns>加载的资源对象</returns>
        public T LoadAsset<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        /// <summary>
        /// 异步加载资源对象
        /// </summary>
        /// <typeparam name="T">要加载的资源类型</typeparam>
        /// <param name="path">资源的路径</param>
        /// <param name="callBack">加载完成后的回调函数</param>
        public void LoadAssetAsync<T>(string path, Action<T> callBack) where T : Object
        {
            StartCoroutine(DoLoadAssetAsync(path, callBack));
        }

        /// <summary>
        /// 异步加载资源对象
        /// </summary>
        /// <typeparam name="T">要加载的资源类型</typeparam>
        /// <param name="path">资源的路径</param>
        /// <param name="callBack">加载完成后的回调函数</param>
        IEnumerator DoLoadAssetAsync<T>(string path, Action<T> callBack) where T : Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(path);
            yield return request;
            callBack?.Invoke(request.asset as T);
        }

        /// <summary>
        /// 异步加载游戏对象
        /// </summary>
        /// <typeparam name="T">要加载的组件类型</typeparam>
        /// <param name="path">预制体的资源路径</param>
        /// <param name="callBack">加载完成后的回调函数</param>
        /// <param name="parent">父对象的Transform（可选）</param>
        public void LoadGameObjAsync<T>
            (string path, Action<T> callBack = null, Transform parent = null) where T : Object
        {
            if (CheckCacheDict(typeof(T))) // 检查指定组件类型是否已经缓存
            {
                // 尝试从对象池中获取已缓存的游戏对象
                GameObject gameObj = PoolManager.Instance.CheckCacheAndLoadGameObj(path, parent);
                if (gameObj != null)
                {
                    // 如果成功获取到游戏对象，则直接调用回调函数，并传递获取到的组件实例
                    callBack?.Invoke(gameObj.GetComponent<T>());
                }
                else
                {
                    // 如果未获取到游戏对象，则启动协程进行异步加载
                    StartCoroutine(DoLoadGameObjAsync(path, callBack, parent));
                }
            }
            else
            {
                // 如果指定组件类型未缓存，则启动协程进行异步加载
                StartCoroutine(DoLoadGameObjAsync(path, callBack, parent));
            }
        }

        /// <summary>
        /// 异步加载游戏对象
        /// </summary>
        /// <typeparam name="T">要加载的组件类型</typeparam>
        /// <param name="path">预制体的资源路径</param>
        /// <param name="callBack">加载完成后的回调函数</param>
        /// <param name="parent">父对象的Transform（可选）</param>
        IEnumerator DoLoadGameObjAsync<T>
            (string path, Action<T> callBack = null, Transform parent = null) where T : Object
        {
            // 异步加载预制体资源
            ResourceRequest request = Resources.LoadAsync<GameObject>(path);
            yield return request;
            GameObject gameObj = InstantiatePrefab(request.asset as GameObject, parent);

            // 调用回调函数，并传递实例化的游戏对象的组件实例
            callBack?.Invoke(gameObj.GetComponent<T>());
        }

        /// <summary>
        /// 通过路径获取预制体对象
        /// </summary>
        /// <param name="path">预制体的资源路径</param>
        /// <returns>预制体对象</returns>
        public GameObject GetPrefab(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            if (prefab != null)
            {
                return prefab;
            }
            throw new Exception("预制体路径有误，没有找到预制体");
        }

        /// <summary>
        /// 根据路径实例化预制体对象
        /// </summary>
        /// <param name="path">预制体的资源路径</param>
        /// <param name="parent">父对象的Transform（可选）</param>
        /// <returns>实例化后的预制体对象</returns>
        public GameObject InstantiatePrefab(string path, Transform parent = null)
        {
            return InstantiatePrefab(GetPrefab(path), parent);
        }

        /// <summary>
        /// 实例化预制体对象
        /// </summary>
        /// <param name="prefab">要实例化的预制体对象</param>
        /// <param name="parent">父对象的Transform（可选）</param>
        /// <returns>实例化后的预制体对象</returns>
        public GameObject InstantiatePrefab(GameObject prefab, Transform parent = null)
        {
            GameObject gameObj = Instantiate(prefab, parent);
            gameObj.name = prefab.name;
            return gameObj;
        }
    }
}