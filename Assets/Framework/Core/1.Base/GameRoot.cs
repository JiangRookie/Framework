using Framework;
using Sirenix.OdinInspector;

public class GameRoot : PersistentMonoSingleton<GameRoot>
{
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