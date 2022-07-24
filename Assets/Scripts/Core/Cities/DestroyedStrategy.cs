using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Cities
{
    public class DestroyedStrategy : MonoBehaviour, ICityStrategy
    {
        private CityScript _city;

        public CityScript City => _city;
        public bool Interactable { get; set; }

        private void Start()
        {
            _city = GetComponent<CityScript>();
            _city.Interactable = false;
        }
    }
}
