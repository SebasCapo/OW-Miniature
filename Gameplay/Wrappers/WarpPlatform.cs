using OWMiniature.Utils;

namespace OWMiniature.Gameplay.Wrappers
{
    public class WarpPlatform(NomaiWarpTransmitter transmitter)
    {
        public static WarpPlatform Instance { get; private set; }

        /// <summary>
        /// The <see cref="NomaiWarpTransmitter"/> instance this is controlling.
        /// </summary>
        public NomaiWarpTransmitter Transmitter { get; } = transmitter;

        public static void Generate()
        {
            foreach (AstroObject astro in PlanetaryUtils.AstroObjects)
            {
                NomaiWarpTransmitter transmitter = astro.GetComponentInChildren<NomaiWarpTransmitter>();

                if (transmitter == null)
                    continue;

                Instance = new WarpPlatform(transmitter);
            }
        }
    }
}
