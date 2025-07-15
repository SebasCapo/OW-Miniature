using UnityEngine;

namespace OWMiniature.Utils
{
    public static class VisualUtils
    {
        private const string NormalLineFile = "Effects_SPA_OrbitLine_mat";
        private const string DottedLineFile = "Effects_SPA_OrbitLine_Dotted_mat";

        public static Material NormalLine
        {
            get
            {
                if (_normalLine == null)
                    _normalLine = FetchResource<Material>(NormalLineFile);

                return _normalLine;
            }
        }

        public static Material DottedLine
        {
            get
            {
                if (_dottedLine == null)
                    _dottedLine = FetchResource<Material>(DottedLineFile);

                return _dottedLine;
            }
        }

        private static Material _normalLine;
        private static Material _dottedLine;

        /// <summary>
        /// Creates a <see cref="Gradient"/> between <paramref name="startColor"/> and <paramref name="endColor"/>.
        /// </summary>
        public static Gradient GenerateGradient(Color startColor, Color endColor)
        {
            Gradient gradient = new Gradient();

            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0].time = 0f;
            colorKeys[0].color = startColor;

            colorKeys[1].time = 1f;
            colorKeys[1].color = endColor;

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[3];
            alphaKeys[0].time = 0f;
            alphaKeys[0].alpha = 1f;

            alphaKeys[1].time = .85f;
            alphaKeys[1].alpha = 1f;

            alphaKeys[2].time = 1f;
            alphaKeys[2].alpha = .0f;

            gradient.colorKeys = colorKeys;
            gradient.alphaKeys = alphaKeys;

            return gradient;
        }

        /// <summary>
        /// Fetches a specific resource from Unity's database.
        /// </summary>
        /// <typeparam name="T">The <see cref="Object"/> that will be fetched.</typeparam>
        /// <param name="resourceName">The file name that will be fetched. Case insensitive.</param>
        /// <returns>The found resource's instance.</returns>
        public static T FetchResource<T>(string resourceName)
            where T : Object
        {
            T[] foundResources = Resources.FindObjectsOfTypeAll<T>();
            
            for (int i = 0; i < foundResources.Length; i++)
            {
                T obj = foundResources[i];

                if (!obj.name.Equals(resourceName, System.StringComparison.OrdinalIgnoreCase))
                    continue;

                return foundResources[i];
            }

            return null;
        }

        /// <summary>
        /// Quicker way to get a color with a specific alpha.
        /// </summary>
        /// <returns>The exact same specified <paramref name="color"/>, but with the specified <paramref name="color"/>.</returns>
        public static Color WithAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }
}
