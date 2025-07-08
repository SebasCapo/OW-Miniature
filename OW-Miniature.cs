using System.Reflection;

using HarmonyLib;

using OWMiniature.Experiments;

using OWML.Common;
using OWML.ModHelper;

namespace OWMiniature;

public class OWMiniature : ModBehaviour
{
    public static OWMiniature Instance;
    public ExperimentManager ExperimentManager;
    public INewHorizons NewHorizons;

    public void Awake()
    {
        Instance = this;
        ExperimentManager = new ExperimentManager();
        // You won't be able to access OWML's mod helper in Awake.
        // So you probably don't want to do anything here.
        // Use Start() instead.
    }

    public void Start()
    {
        // Starting here, you'll have access to OWML's mod helper.
        ModHelper.Console.WriteLine($"My mod {nameof(OWMiniature)} is loaded!", MessageType.Success);

        // Get the New Horizons API and load configs
        NewHorizons = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
        NewHorizons.LoadConfigs(this);

        ExperimentManager.Setup();
        new Harmony("Ender.OW-Miniature").PatchAll(Assembly.GetExecutingAssembly());

        // Example of accessing game code.
        OnCompleteSceneLoad(OWScene.TitleScreen, OWScene.TitleScreen); // We start on title screen
        LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;
    }

    public void OnCompleteSceneLoad(OWScene previousScene, OWScene newScene)
    {
        if (newScene != OWScene.SolarSystem) return;
        ModHelper.Console.WriteLine("Loaded into solar system!", MessageType.Success);
    }
}

