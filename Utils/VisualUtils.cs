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
    }
}
