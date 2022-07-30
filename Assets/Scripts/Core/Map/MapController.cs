using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Zenject;
using DG.Tweening;
using Core.Infrastructure;
using Core.Cities;

namespace Core
{
    [RequireComponent(typeof(Collider2D))]
    public class MapController : MonoBehaviour
    {
        [Header("Dissolve")]
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private CircleCollider2D _circleCollider;
        [SerializeField, Min(0f)]
        private float _dissolveDuration;

        private SignalBus _signalBus;
        private float _colliderStartRadius;
        private Material _material;
        private static LinkedList<CityScript> _cities = new LinkedList<CityScript>();

        public IReadOnlyCollection<CityScript> Cities => _cities;

        [Inject]
        private void Construct(SignalBus signalBus) => _signalBus = signalBus;

        private void Awake()
        {
            _signalBus.Subscribe<GameApocalypseSignal>(OnGameApocalypse);
        }
        private void Start()
        {
            _material = _spriteRenderer.material;
            _colliderStartRadius = _circleCollider.radius;
        }
        private void OnDestroy()
        {
            _signalBus.Unsubscribe<GameApocalypseSignal>(OnGameApocalypse);
        }

        private void OnGameApocalypse()
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
        public void SetAllCitiesInteractable(bool isInteractable)
        {
            foreach (CityScript city in _cities)
            {
                city.Interactable = isInteractable;
            }
        }    
       
        public static void RegisterCity(CityScript city)
        {
            if (!_cities.Contains(city)) _cities.AddLast(city);
        }
        public static void UnregisterCity(CityScript city)
        {
            _cities.Remove(city);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            CityScript city = collision.GetComponent<CityScript>();
            if (city != null)
            {
                city.DestroyCity();
            }
        }
    }
}