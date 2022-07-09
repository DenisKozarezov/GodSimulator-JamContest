using UnityEngine;

namespace Core
{
    public static class ColorExtensions
    {
        public static Color SetAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }
}