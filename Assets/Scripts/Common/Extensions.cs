using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Core.Cities;

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
        public static IEnumerable<CityScript> SelectMany(this IEnumerable<CityScript> cities, Func<CityScript, bool> selector)
        {
            foreach (var city in cities)
            {
                if (selector(city)) yield return city;
            }
        }
        public static IEnumerable<T> SelectMany<T>(this IEnumerable<CityScript> cities, Func<T, bool> selector) where T : ICityStrategy
        {
            foreach (var city in cities)
            {
                if (city.TryGetComponent(out T value) && selector(value)) yield return value;
            }
        }
        public static IEnumerable<T> ByDistance<T>(this IEnumerable<T> cities, float3 position, float distance) where T : MonoBehaviour
        {
            foreach (var city in cities)
            {
                if (MathUtils.CheckDistance(city.transform.position, position, distance))
                    yield return city;
            }
        }
        public static IEnumerable<CityScript> ByOwner(this IEnumerable<CityScript> cities, Player owner)
        {
            foreach (var city in cities)
            {
                if (city.Owner.Equals(owner)) yield return city;
            }
        }
        public static IEnumerable<T> ByOwner<T>(this IEnumerable<T> cities, Player owner)
          where T : MonoBehaviour, ICityStrategy
        {
            foreach (var city in cities)
            {
                if (city.City.Owner.Equals(owner)) yield return city;
            }
        }
        public static T Randomly<T>(this IEnumerable<T> cities)
        {
            int count = cities.Count();
            int rand = MathUtils.Random.NextInt(0, count);
            IEnumerator<T> iterator = cities.GetEnumerator();
            iterator.MoveNext();
            for (int i = 0; i < count; i++)
            {
                if (i == rand) break;
                iterator.MoveNext();
            }
            return iterator.Current;
        }
        public static IEnumerable<T> Randomly<T>(this IEnumerable<T> cities, byte count)
        {
            return cities.OrderBy(x => MathUtils.Random.NextInt()).Take(count);
        }
        public static IEnumerable<CityScript> Jarvis(this IEnumerable<CityScript> cities)
        {
            if (cities.Count() <= 2) yield break;

            CityScript leftest = cities.OrderBy(x => x.transform.position.x).First(); 
            CityScript current = leftest;
            CityScript endPoint;
            bool skipped = false;
            do
            {
                yield return current;
                endPoint = leftest;

                foreach (CityScript city in cities)
                {
                    if (!skipped)
                    {
                        skipped = true;
                        continue;
                    }

                    if (current.Equals(endPoint) || (MathUtils.Orientation(current.transform.position, endPoint.transform.position, city.transform.position) == -1))
                    {
                        endPoint = city;
                    }
                }
                current = endPoint;
            }
            while (!endPoint.Equals(leftest));
        }
        public static IEnumerable<T> Jarvis<T>(this IEnumerable<T> cities) 
            where T : MonoBehaviour, ICityStrategy
        {
            if (cities.Count() <= 2) yield break;

            T leftest = cities.OrderBy(x => x.transform.position.x).First();
            T current = leftest;
            T endPoint;
            bool skipped = false;
            do
            {
                yield return current;
                endPoint = leftest;

                foreach (T city in cities)
                {
                    if (!skipped)
                    {
                        skipped = true;
                        continue;
                    }

                    if (current.Equals(endPoint) || (MathUtils.Orientation(current.transform.position, endPoint.transform.position, city.transform.position) == -1))
                    {
                        endPoint = city;
                    }
                }
                current = endPoint;
            }
            while (!endPoint.Equals(leftest));
        }
    }
}