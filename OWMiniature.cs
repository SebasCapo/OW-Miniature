using System;
using System.Collections;
using System.Reflection;

using HarmonyLib;

using OWMiniature.Experiments;
using OWMiniature.Gameplay;
using OWMiniature.Gameplay.Interactables;
using OWMiniature.Gameplay.Spawnables;
using OWMiniature.Gameplay.Wrappers;
using OWMiniature.Utils;

using OWML.Common;
using OWML.ModHelper;

using UnityEngine;

namespace OWMiniature;

public class OWMiniature : ModBehaviour
{
    private const string SolarSystemName = "Jam5";

    public static OWMiniature Instance;
    public static IModConsole Console;
    public ExperimentManager ExperimentManager;
    public INewHorizons NewHorizons;

    public static event Action OnLoadComplete;

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
        Console = ModHelper.Console;
        Console.WriteLine($"My mod {nameof(OWMiniature)} is loaded!", MessageType.Success);

        // Get the New Horizons API and load configs
        NewHorizons = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
        NewHorizons.LoadConfigs(this);
        NewHorizons.GetStarSystemLoadedEvent().AddListener(OnSystemLoaded);

        ExperimentManager.Setup();
        TargetTrackingHandler.Register();

        new Harmony("Ender.OW-Miniature").PatchAll(Assembly.GetExecutingAssembly());

        // Example of accessing game code.
        OnCompleteSceneLoad(OWScene.TitleScreen, OWScene.TitleScreen); // We start on title screen
        LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;
    }

    private void OnSystemLoaded(string starSystem)
    {
        if (!starSystem.Equals(SolarSystemName))
            return;

        CrystaliaStation.Generate();
        EnergyReplicator.Generate();
        StarController.Generate();
        WarpPlatform.Generate();
        NeemVessel.Generate();
        EnergyController.Generate();

        // These had to be spawned with a few seconds delay, I don't understand why, I hate it.
        StartCoroutine(SpawnDelayed(3.5f));
    }

    private IEnumerator SpawnDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        MapInteractableBase.Attach<EnergyReplicatorTerminal>();
        MapInteractableBase.Attach<WarpTerminal>();
        MapInteractableBase.Attach<ConnectionsMap>();
    }

    public void OnCompleteSceneLoad(OWScene previousScene, OWScene newScene)
    {
        MapUtils.ResetCache();
        VisualUtils.ResetCache();
        PlanetaryUtils.ResetCache();

        if (newScene != OWScene.SolarSystem)
            return;

        ModHelper.Console.WriteLine("Loaded into solar system!", MessageType.Success);
    }
}

