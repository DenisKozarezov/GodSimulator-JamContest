using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Core.Infrastructure;
using Core.UI;
using DG.Tweening;
using System.Threading.Tasks;

namespace Core.Cities
{
    public class MovingBetweenCities : IInitializable, ILateDisposable
    {
        private readonly SignalBus _signalBus;
        private IEnumerable<Collider2D> _colliders;

        private AnimatedDottedLine _linePrefab;
        private MovingPriestsIcon _iconPrefab;

        public MovingBetweenCities(SignalBus signalBus, DiContainer container)
        {
            _signalBus = signalBus;
            _linePrefab = container.Resolve<AnimatedDottedLine>();
            _iconPrefab = container.Resolve<MovingPriestsIcon>();
        }
        void IInitializable.Initialize()
        {
            _signalBus.Subscribe<TempleDragBeginSignal>(OnTempleDragBegin);
            _signalBus.Subscribe<PlayerMovingPriestsSignal>(OnPlayerMovingPriests);
        }
        void ILateDisposable.LateDispose()
        {
            _signalBus.Unsubscribe<TempleDragBeginSignal>(OnTempleDragBegin);
            _signalBus.Unsubscribe<PlayerMovingPriestsSignal>(OnPlayerMovingPriests);
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
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(signal.Temple.transform.position, signal.Temple.Range, Constants.CitiesLayer);
            Collider2D selfCollider = signal.Temple.GetComponent<Collider2D>();
            _colliders = from collider in colliderArray
                         where collider != selfCollider
                         select collider;
            SelectCities(_colliders);
        }
        private void OnPlayerMovingPriests(PlayerMovingPriestsSignal signal)
        {
            Vector2 startPos = signal.Temple.transform.position;
            Vector2 endPos = signal.Target.transform.position;

            var line = CreateAnimatedDottedLine(startPos, endPos, signal.Duration);
            var icon = CreateMovingIcon(startPos, endPos, signal.Duration);
            icon.SetAmount(signal.PriestsAmount);

            //Move(signal, startPos, endPos);
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
        private async void Move(PlayerMovingPriestsSignal signal, Vector2 startPos, Vector2 endPos)
        {
            //signal.Temple.City.ReducePriests(signal.God, signal.PriestsAmount);
            Debug.Log(endPos - startPos);
            await Task.Delay(5000); //signal.Duration
            //signal.Target.AddPriests(signal.God, signal.PriestsAmount);
        }
    }
}