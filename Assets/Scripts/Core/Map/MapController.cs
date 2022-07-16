using System.Collections.Generic;
using UnityEngine;
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
        private LinkedList<CityScript> _cities;

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
            _circleCollider.radius = Mathf.Lerp(0f, _colliderStartRadius, value);
        }
        public void StartDissolve()
        {
            DOTween.To(() => 1f, x => SetDissolveValue(x), 0f, _dissolveDuration).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _circleCollider.enabled = false;
            });
        }
    }
}