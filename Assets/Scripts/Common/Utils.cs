using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Mathematics;

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
        public static bool IsPointerOverGameObject()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
        public static Vector2 GetCorrectedPosition(RectTransform first, RectTransform second, float margin = 5f)
        {
            float x = first.position.x;
            float y = first.position.y + first.sizeDelta.y + second.sizeDelta.y / 2 + margin;
            return new Vector2(x, y);
        }
        public static Vector2 WorldToScreenPoint(Vector3 position)
        {
            return Camera.main.WorldToScreenPoint(position);
        }
        public static Vector3 ScreenToWorldPoint(Vector2 position)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            RaycastHit2D[] raycastHits = new RaycastHit2D[1];
            int hits = Physics2D.RaycastNonAlloc(ray.origin, ray.direction, raycastHits, Mathf.Infinity, ~0);
            
            if (hits == 0) return Vector3.zero;
            
            return raycastHits[0].point;
        }
        public static Lazy<IEnumerable<T>> CreateLazyArray<T>(string path) where T : UnityEngine.Object
        {
            return new Lazy<IEnumerable<T>>(() => UnityEngine.Resources.LoadAll<T>(path));
        }
        public static Lazy<IEnumerable<T>> CreateLazyArray<T>(IEnumerable<T> collection)
        {
            return new Lazy<IEnumerable<T>>(() => collection);
        }
    }
    public static class MathUtils
    {
        private static Unity.Mathematics.Random _random;
        public static Unity.Mathematics.Random Random => _random;
        static MathUtils()
        {
            _random = new Unity.Mathematics.Random();
            _random.InitState(unchecked((uint)DateTime.Now.Ticks));
        }
        public static float Distance(float2 first, float2 second)
        {
            return math.distance(first, second);
        }
        public static float Distance(float3 first, float3 second)
        {
            return math.distance(first, second);
        }
        public static float DistanceSqr(float2 first, float2 second)
        {
            return math.distancesq(first, second);
        }
        public static float DistanceSqr(float3 first, float3 second)
        {
            return math.distancesq(first, second);
        }
        public static bool CheckDistance(float2 first, float2 second, float distance)
        {
            return DistanceSqr(first, second) <= math.pow(distance, 2);
        }
        public static bool CheckDistance(float3 first, float3 second, float distance)
        {
            return DistanceSqr(first, second) <= math.pow(distance, 2);
        }
    }
}