using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using DG.Tweening;
using Core.Infrastructure;
using Core.UI;
using System.Threading.Tasks;

namespace Core.Cities
{
    public class MovingBetweenCities : IInitializable, ILateDisposable
    {
        private readonly SignalBus _signalBus;
        private IEnumerable<Collider2D> _colliders;

        private TempleRadius _radiusPrefab;
        private AnimatedDottedLine _linePrefab;
        private MovingPriestsIcon _iconPrefab;

        private TempleRadius _radius;

        public MovingBetweenCities(SignalBus signalBus, DiContainer container)
        {
            _signalBus = signalBus;
            _radiusPrefab = container.Resolve<TempleRadius>();
            _linePrefab = container.Resolve<AnimatedDottedLine>();
            _iconPrefab = container.Resolve<MovingPriestsIcon>();
        }
        void IInitializable.Initialize()
        {
            _signalBus.Subscribe<TempleDragBeginSignal>(OnTempleDragBegin);
            _signalBus.Subscribe<TempleDragEndSignal>(OnTempleDragEndSignal);
            _signalBus.Subscribe<PlayerMovingPriestsSignal>(OnPlayerMovingPriests);
        }
        void ILateDisposable.LateDispose()
        {
            _signalBus.Unsubscribe<TempleDragBeginSignal>(OnTempleDragBegin);
            _signalBus.Unsubscribe<TempleDragEndSignal>(OnTempleDragEndSignal);
            _signalBus.Unsubscribe<PlayerMovingPriestsSignal>(OnPlayerMovingPriests);
        }

        private TempleRadius CreateTempleVisibleRadius(Transform templeTransform, float range)
        {
            var radius = GameObject.Instantiate(_radiusPrefab, templeTransform);
            radius.SetRange(range);
            return radius;
        }
        private AnimatedDottedLine CreateAnimatedDottedLine(Vector2 startPos, Vector2 endPos, float time)
        {
            var line = GameObject.Instantiate(_linePrefab);
            line.StartPosition = startPos;
            line.EndPosition = endPos;
            GameObject.Destroy(line.gameObject, time);
            return line;
        }
        private MovingPriestsIcon CreateMovingIcon(Vector2 startPos, Vector2 endPos, float time)
        {
            var icon = GameObject.Instantiate(_iconPrefab, startPos, Quaternion.identity);
            icon.transform.DOMove(endPos, time).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                GameObject.Destroy(icon.gameObject);
            });
            return icon;
        }
        private void OnTempleDragBegin(TempleDragBeginSignal signal)
        {
            if (_colliders != null && _colliders.Count() != 0)
            {
                DeselectCities();
            }
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(signal.Temple.transform.position, signal.Temple.Range, 1 << Constants.CitiesLayer);
            Collider2D selfCollider = signal.Temple.GetComponent<Collider2D>();
            _colliders = from collider in colliderArray
                         where collider != selfCollider
                         select collider;
            SelectCities(_colliders);
            _radius = CreateTempleVisibleRadius(signal.Temple.transform, signal.Temple.Range);
        }
        private void OnTempleDragEndSignal(TempleDragEndSignal signal)
        {
            GameObject.Destroy(_radius.gameObject);
        }
        private async void OnPlayerMovingPriests(PlayerMovingPriestsSignal signal)
        {
            Vector2 startPos = signal.Temple.transform.position;
            Vector2 endPos = signal.Target.transform.position;

            signal.Temple.City.ReducePriests(signal.Temple.City.Invader, signal.PriestsAmount);

            var line = CreateAnimatedDottedLine(startPos, endPos, signal.Duration);
            var icon = CreateMovingIcon(startPos, endPos, signal.Duration);
            icon.SetAmount(signal.PriestsAmount);

            await Task.Delay((int)signal.Duration * 1000);
            signal.Target.AddPriests(signal.Temple.City.Invader, signal.PriestsAmount);
        }
        private void SelectCities(IEnumerable<Collider2D> colliders)
        {
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out SpriteRenderer renderer))
                {
                    renderer.color = Color.green;
                }
            }
        }
        private void DeselectCities()
        {
            foreach (var collider in _colliders)
            {
                if (collider.TryGetComponent(out SpriteRenderer renderer))
                {
                    renderer.color = Color.white;
                }
            }
        }
    }
}