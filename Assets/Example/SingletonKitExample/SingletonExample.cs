using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Example
{
    public class NormalSingletonExample : Singleton<NormalSingletonExample>
    {
        public string Text = "NormalSingletonExample";
    }

    public class MonoSingletonExample : MonoSingleton<MonoSingletonExample> // 需要挂载到GameObject上才可以生效
    {
        public string Text = "MonoSingletonExample";
    }

    public class SingletonExample : MonoBehaviour
    {
        [Button("NormalSingletonExample")]
        void GetNormalSingletonExample()
        {
            Debug.Log(NormalSingletonExample.Instance.Text);
        }

        [Button("MonoSingletonExample")]
        void GetMonoSingletonExample()
        {
            Debug.Log(MonoSingletonExample.Instance.Text);
        }
    }
}