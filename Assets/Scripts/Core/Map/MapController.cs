using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Zenject;
using DG.Tweening;
using Core.Infrastructure;
using Core.Cities;

namespace Core
{
    public class MapController : MonoBehaviour
    {
        [Header("Dissolve")]
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private CircleCollider2D _circleCollider;
        [SerializeField, Min(0f)]
        private float _dissolveDuration;

        private float _colliderStartRadius;
        private Material _material;

        private SignalBus _signalBus;
        private static Dictionary<uint, LinkedList<CityScript>> _cities 
            = new Dictionary<uint, LinkedList<CityScript>>();

        public IReadOnlyDictionary<uint, LinkedList<CityScript>> Cities => _cities;

        [Inject]
        public void Construct(SignalBus signalBus) => _signalBus = signalBus;

        private void Awake()
        {
            _signalBus.Subscribe<GameApocalypsisSignal>(OnGameApocalypsis);
        }
        private void Start()
        {
            _material = _spriteRenderer.material;
            _colliderStartRadius = _circleCollider.radius;
        }
        private void OnDestroy()
        {
            _signalBus.Unsubscribe<GameApocalypsisSignal>(OnGameApocalypsis);
        }

        private void OnGameApocalypsis()
        {
            StartDissolve();
        }

        private void SetDissolveValue(float value)
        {
            _material.SetFloat("_DissolveValue", value);
            _circleCollider.radius = math.lerp(0f, _colliderStartRadius, value);
        }
        private void StartDissolve()
        {
            DOTween.To(() => 1f, x => SetDissolveValue(x), 0f, _dissolveDuration).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _circleCollider.enabled = false;
            });
        }
        public static void RegisterCity(CityScript city)
        {
            if (!_cities.ContainsKey(city.Invader.ID))
            {    
                _cities.Add(city.Invader.ID, new LinkedList<CityScript>());
            }
            _cities[city.Invader.ID].AddLast(city);
        }
        public static void UnregisterCity(CityScript city)
        {
            if (_cities.ContainsKey(city.Invader.ID))
            {
                _cities[city.Invader.ID].Remove(city);
            }
        }
    }
}