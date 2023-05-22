using Framework;
using UnityEngine;

public class ConfigManager : ManagerBase<ConfigManager>
{
    [SerializeField] ConfigSetting m_ConfigSetting;

    public T Get<T>(string configName, int id) where T : ConfigBase
    {
        return m_ConfigSetting.Get<T>(configName, id);
    }
}