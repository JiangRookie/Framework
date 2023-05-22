using Sirenix.OdinInspector;
using UnityEngine;

namespace Example
{
    public class ConfigKitExample : MonoBehaviour
    {
        [Button("GetConfigTest1")]
        void GetConfigTest1()
        {
            Debug.Log(ConfigManager.Instance.Get<Config1>("Test", 0));
            Debug.Log(ConfigManager.Instance.Get<Config1>("Test", 1));

            // Exception: 配置字典中不存在类型：Test1
            Debug.Log(ConfigManager.Instance.Get<Config1>("Test1", 0));
        }

        [Button("GetConfigTest2")]
        void GetConfigTest2()
        {
            // Exception: 类型：Test的配置字典中不存在id：2
            Debug.Log(ConfigManager.Instance.Get<Config1>("Test", 2));
        }
    }
}