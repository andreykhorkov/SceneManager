using Zenject;

public class MainManagerInstaller : MonoInstaller<MainManagerInstaller>
{
    public MainManager mainManager;

    public override void InstallBindings()
    {
        Container.BindInstance(mainManager);
    }
}