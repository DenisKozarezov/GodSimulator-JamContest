using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core.UI;

namespace Core.Cities
{
    public class NeutralStrategy : MonoBehaviour, ICityStrategy
    {
        [Header("Neutral City")]
        [SerializeField, Min(0f)]
        private float _faithRate;
        [SerializeField, Min(0f)]
        private float _growthOfFaith;

        private class LightWeightPair
        {
            public Player Owner;
            public float Faith;
        }

        private List<LightWeightPair> _competitors = new List<LightWeightPair>();
        private PranaView _pranaView;
        private CityScript _city;
        private float _timer;
        private bool _rating;

        public CityScript City => _city;
        public bool Interactable { get; set; }
        public float Total => _competitors.Sum(x => x.Faith);

        public void Construct(PranaView pranaView)
        {
            _pranaView = pranaView;
        }
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

                for (int i = 0; i < _competitors.Count; i++)
                {
                    _competitors[i].Faith += _growthOfFaith;
                    _pranaView.SetFillAmount(_competitors[i].Faith * 0.01f);
                }
                _timer = _faithRate;
            }
        }
        void ICityStrategy.Disable()
        {
            _rating = false;
        }

        public void AddNewGodForFaithfull(Player god)
        {
            if (god == null) return;

            if (_competitors.FindIndex(x => x.Owner.Equals(god)) == -1)
            {
                _competitors.Add(new LightWeightPair { Owner = god, Faith = 0f });
                _rating = true;
            }
        }

        public bool Equals(ICityStrategy other)
        {
            if (other == null) return false;
            return _city.Equals(other.City);
        }
    }
}