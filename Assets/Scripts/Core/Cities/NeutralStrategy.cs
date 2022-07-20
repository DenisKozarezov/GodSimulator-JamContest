using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core.Models;

namespace Core.Cities
{
    public class NeutralStrategy : MonoBehaviour, ICityStrategy
    {
        [Header("Neutral City")]
        [SerializeField, Min(0f)]
        private float _growthRate;
        [SerializeField, Min(0f)]
        private float _growthOfFaith;

        private Dictionary<GodModel, float> _percentageOfFaithful = new Dictionary<GodModel, float>();
        private CityScript _city;
        private float _timer;
        private bool _rating;

        public CityScript City => _city;
        public bool Interactable { get; set; }
        public float Total => _percentageOfFaithful.Values.Sum(x => x);

        private void Start()
        {
            _city = GetComponent<CityScript>();
        }
        private void Update()
        {
            if (!_rating) return;

            if (_timer > 0f) _timer -= Time.deltaTime;
            else
            {
                if (Total >= Constants.FaithfulValueMax) return;

                foreach (var faith in _percentageOfFaithful)
                {
                    if (_percentageOfFaithful.TryGetValue(faith.Key, out float value))
                    {
                        value += _growthOfFaith;
                    }
                }
                _timer = _growthRate;
            }
        }

        public void AddNewGodForFaithfull(GodModel god)
        {
            if (!_percentageOfFaithful.ContainsKey(god))
            {
                _percentageOfFaithful.Add(god, 0);
                _rating = true;
            }
        }
    }
}