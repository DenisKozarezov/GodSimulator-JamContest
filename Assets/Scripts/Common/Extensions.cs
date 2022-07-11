using UnityEngine;

namespace Core
{
    public static class ColorExtensions
    {
        public static Color WithR(this Color color, float r)
        {
            color.r = r;
            return color;
        }
        public static Color WithG(this Color color, float g)
        {
            color.g = g;
            return color;
        }
        public static Color WithB(this Color color, float b)
        {
            color.b = b;
            return color;
        }
        public static Color WithAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }
}