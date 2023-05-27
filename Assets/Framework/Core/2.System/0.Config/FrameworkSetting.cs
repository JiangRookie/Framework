using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Framework
{
    [CreateAssetMenu(fileName = "FrameworkSetting", menuName = "Framework/FrameworkSetting")]
    public class FrameworkSetting : ConfigBase
    {
#if UNITY_EDITOR
        [Button(Name = "初始化框架配置", ButtonHeight = 50), GUIColor(0, 1, 0)]
        void Init() { }

        [InitializeOnLoadMethod]
        static void FrameworkSettingInitializer()
        {
            GameObject.Find("GameRoot").GetComponent<GameRoot>().FrameworkSetting.Init();
        }
#endif
    }
}