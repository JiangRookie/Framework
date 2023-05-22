using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework
{
    [CreateAssetMenu(fileName = "ConfigSetting", menuName = "Framework/ConfigSetting")]
    public class ConfigSetting : ConfigBase
    {
        /// <summary>
        /// 配置字典，用于存储不同类型名称的配置字典
        /// </summary>
        [DictionaryDrawerSettings(KeyLabel = "类型名称", ValueLabel = "配置字典")]
        public Dictionary<string, Dictionary<int, ConfigBase>> ConfigDict;

        /// <summary>
        /// 根据类型名称和配置id获取对应的配置对象
        /// </summary>
        /// <typeparam name="T">配置对象的类型</typeparam>
        /// <param name="configName">类型名称</param>
        /// <param name="id">配置id</param>
        /// <returns>对应的配置对象</returns>
        /// <exception cref="Exception">当类型名称不存在或配置id不存在时会抛出异常</exception>
        public T Get<T>(string configName, int id) where T : ConfigBase
        {
            // 检查配置字典中是否包含指定的类型名称
            if (ConfigDict.TryGetValue(configName, out var configTypeDict) == false)
            {
                throw new Exception($"配置字典中不存在类型：{configName}");
            }

            // 检查指定类型名称的配置字典中是否包含指定的配置id
            if (configTypeDict.TryGetValue(id, out var config) == false)
            {
                throw new Exception($"类型：{configName}的配置字典中不存在id：{id}");
            }

            // 尝试将配置对象转换为泛型类型T
            if (config is T result)
            {
                return result;
            }

            // 如果无法将配置对象转换为泛型类型T，则抛出异常
            throw new Exception($"类型：{configName}的配置字典中的id：{id}无法转换为类型：{typeof(T).Name}");
        }
    }
}