using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Core.Cities;
using Core.Models;

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
    public static class CitiesExtensions
    {
        public static IEnumerable<CityScript> ByDistance(this IEnumerable<CityScript> cities, float3 position, float distance)
        {
            foreach (var city in cities)
            {
                if (MathUtils.CheckDistance(city.transform.position, position, distance))
                    yield return city;
            }
        }
        public static IEnumerable<T> ByDistance<T>(this IEnumerable<T> cities, float3 position, float distance)
            where T : MonoBehaviour, ICityStrategy
        {
            foreach (var city in cities)
            {
                if (city.TryGetComponent(out T value) && MathUtils.CheckDistance(city.transform.position, position, distance))
                    yield return value;
            }
        }
        public static IEnumerable<CityScript> ByOwner(this IEnumerable<CityScript> cities, GodModel owner)
        {
            foreach (var city in cities)
            {
                if (city.Invader.Equals(owner)) yield return city;
            }
        }
        public static IEnumerable<T> ByOwner<T>(this IEnumerable<T> cities, GodModel owner)
          where T : MonoBehaviour, ICityStrategy
        {
            foreach (var city in cities)
            {
                if (city.TryGetComponent(out T value) && value.City.Invader.Equals(owner))
                    yield return value;
            }
        }
        public static IEnumerable<CityScript> ByOwner(this IEnumerable<CityScript> cities, uint id)
        {
            foreach (var city in cities)
            {
                if (city.Invader == id) yield return city;
            }
        }
        public static IEnumerable<T> ByOwner<T>(this IEnumerable<T> cities, uint id)
          where T : MonoBehaviour, ICityStrategy
        {
            foreach (var city in cities)
            {
                if (city.TryGetComponent(out T value) && value.City.Invader == id)
                    yield return value;
            }
        }
    }
}