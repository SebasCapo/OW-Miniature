using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using static UnityEngine.GraphicsBuffer;

namespace OWMiniature.Utils
{
    public static class MiscUtils
    {
        public static bool TryGetChildByName(this Transform transform, string name, out Transform child)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                child = transform.GetChild(i);

                if (child.name.StartsWith(name, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            child = null;
            return false;
        }

        public static string GetHierarchyName(this string name)
        {
            // Based on how New Horizons writes name strings.
            return name.Replace(" ", string.Empty).Replace("'", string.Empty) + "_Body";
        }

        public static IEnumerator CallDelayed(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);

            action?.Invoke();
        }
    }
}
