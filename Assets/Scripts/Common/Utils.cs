using System;
using UnityEngine;

namespace Core
{
    public static class Utils
    {
        public static string ParseToRoman(int number)
        {
            if (number < 1 || number > 3999) return string.Empty;
            if (number >= 1000) return "M" + ParseToRoman(number - 1000);
            if (number >= 900) return "CM" + ParseToRoman(number - 900);
            if (number >= 500) return "D" + ParseToRoman(number - 500);
            if (number >= 400) return "CD" + ParseToRoman(number - 400);
            if (number >= 100) return "C" + ParseToRoman(number - 100);
            if (number >= 90) return "XC" + ParseToRoman(number - 90);
            if (number >= 50) return "L" + ParseToRoman(number - 50);
            if (number >= 40) return "XL" + ParseToRoman(number - 40);
            if (number >= 10) return "X" + ParseToRoman(number - 10);
            if (number >= 9) return "IX" + ParseToRoman(number - 9);
            if (number >= 5) return "V" + ParseToRoman(number - 5);
            if (number >= 4) return "IV" + ParseToRoman(number - 4);
            if (number >= 1) return "I" + ParseToRoman(number - 1);
            throw new ArgumentOutOfRangeException();
        }
        public static Vector2 GetCorrectedPosition(RectTransform first, RectTransform second, float margin = 5f)
        {
            float x = first.position.x;
            float y = first.position.y + first.sizeDelta.y + second.sizeDelta.y / 2 + margin;
            return new Vector2(x, y);
        }
    }
}