using System.Reflection;

using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Wrappers
{
    public class StarController : MonoBehaviour
    {
        public static StarController Instance { get; private set; }

        private Component _starEvolutionController;

        public static void Generate()
        {
            Instance = PlanetaryUtils.Sun.gameObject.AddComponent<StarController>();
            
            if (!Instance.transform.TryGetChildByName("Sector", out Transform sector))
            {
                OWMiniature.Console.WriteLine($"Couldn't find Sector in {Instance.name}.");
                return;
            }

            if (!sector.TryGetChildByName("Star", out Transform starControlleroObj))
            {
                OWMiniature.Console.WriteLine($"Couldn't find Sector in {Instance.name}.");
                return;
            }

            Instance._starEvolutionController = starControlleroObj.GetComponent("NewHorizons.Components.SizeControllers.StarEvolutionController");

            if (Instance._starEvolutionController == null)
            {
                OWMiniature.Console.WriteLine($"Couldn't find the StarEvolutionController in {Instance.name}.");
                return;
            }
        }

        public void StartCollapse()
        {
            MethodInfo startCollapseMethod = _starEvolutionController.GetType().GetMethod("StartCollapse", BindingFlags.Instance | BindingFlags.Public);

            if (startCollapseMethod == null)
                return;
            
            startCollapseMethod.Invoke(_starEvolutionController, null);
        }
    }
}
