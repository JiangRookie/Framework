using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameRoot : PersistentMonoSingleton<GameRoot>
{
    [SerializeField] FrameworkSetting m_FrameworkSetting;
    public FrameworkSetting FrameworkSetting => m_FrameworkSetting;

    protected override void Awake()
    {
        base.Awake();
        InitAllManagers();
    }

    [Button("InitAllManagers")]
    void InitAllManagers()
    {
        IManager[] managers = GetComponents<IManager>();
        foreach (var manager in managers)
        {
            manager.Init();
        }
    }
}