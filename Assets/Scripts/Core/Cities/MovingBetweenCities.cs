using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Mathematics;
using Zenject;
using DG.Tweening;
using Core.Infrastructure;
using Core.UI;
using Core.UI.Forms;

namespace Core.Cities
{
    public class MovingBetweenCities : IInitializable, ILateDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly MapController _mapController;
        private Circle _radius;
        private IEnumerable<CityScript> _cities;

        private const string RadiusPrefabPath = "Prefabs/Views/Solid Circle";
        private const string LinePrefabPath = "Prefabs/Cities/Animated Dotted Line";
        private const string IconPrefabPath = "Prefabs/Views/Moving Priests Icon";
        private const string FormPrefab = "Prefabs/Views/Forms/Moving Priests Form";

        public MovingBetweenCities(SignalBus signalBus, MapController mapController)
        {
            _signalBus = signalBus;
            _mapController = mapController;
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

        private Circle CreateTempleVisibleRadius(Vector2 position, float range)
        {
            var asset = Resources.Load<Circle>(RadiusPrefabPath);
            var circle = GameObject.Instantiate(asset, position, Quaternion.identity);
            circle.SetRadius(range);
            return circle;
        }
        private AnimatedDottedLine CreateAnimatedDottedLine(Vector2 startPos, Vector2 endPos, float time)
        {
            var asset = Resources.Load<AnimatedDottedLine>(LinePrefabPath);
            var line = GameObject.Instantiate(asset);
            line.StartPosition = startPos;
            line.EndPosition = endPos;
            GameObject.Destroy(line.gameObject, time);
            return line;
        }
        private MovingPriestsIcon CreateMovingIcon(Vector2 startPos, Vector2 endPos, float time)
        {
            var asset = Resources.Load<MovingPriestsIcon>(IconPrefabPath);
            var icon = GameObject.Instantiate(asset, startPos, Quaternion.identity);
            icon.transform.DOMove(endPos, time).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                GameObject.Destroy(icon.gameObject);
            });
            return icon;
        }
        private MovingPriestsForm CreateMovingPriestsForm(TempleDragEndSignal signal)
        {
            var asset = Resources.Load<MovingPriestsForm>(FormPrefab);
            var form = GameObject.Instantiate(asset);
            form.Init(signal.Temple.City.PriestsAmount);
            return form;
        }
        private void OnTempleDragBegin(TempleDragBeginSignal signal)
        {
            DeselectCities(); 

            Vector3 position = signal.Temple.transform.position;
            float range = signal.Temple.GetRange();

            _cities = _mapController.Cities
                .SelectMany(city => city != signal.Temple)
                .ByDistance(position, range);

            SelectCities(_cities);
            _radius = CreateTempleVisibleRadius(position, range);
        }
        private async void OnTempleDragEndSignal(TempleDragEndSignal signal)
        {
            GameObject.Destroy(_radius.gameObject);

            if (signal.Target == null || signal.Temple.Equals(signal.Target)) return;

            float3 templePos = signal.Temple.transform.position;
            float3 targetPos = signal.Target.transform.position;
            float range = signal.Temple.GetRange();
            if (!MathUtils.CheckDistance(templePos, targetPos, range)) return;

            MovingPriestsForm form = CreateMovingPriestsForm(signal);
            ushort priestsCount = await form.AwaitForConfirm();

            _signalBus.Fire(new PlayerMovingPriestsSignal
            {
                Temple = signal.Temple,
                Target = signal.Target,
                Duration = 10f,
                PriestsAmount = priestsCount
            });
        }
        private async void OnPlayerMovingPriests(PlayerMovingPriestsSignal signal)
        {
            Vector2 startPos = signal.Temple.transform.position;
            Vector2 endPos = signal.Target.transform.position;

            signal.Temple.City.ReducePriests(signal.Temple.City.Owner, signal.PriestsAmount);

            CreateAnimatedDottedLine(startPos, endPos, signal.Duration);
            var icon = CreateMovingIcon(startPos, endPos, signal.Duration);
            icon.SetAmount(signal.PriestsAmount);

            await Task.Delay(TimeSpan.FromSeconds(signal.Duration));
            signal.Target.AddPriests(signal.Temple.City.Owner, signal.PriestsAmount);
        }
        private void SelectCities(IEnumerable<CityScript> cities)
        {
            foreach (var city in cities)
            {
                if (city.TryGetComponent(out SpriteRenderer renderer))
                {
                    renderer.color = Color.green;
                }
            }
        }
        private void DeselectCities()
        {
            if (_cities == null) return;

            foreach (var city in _cities)
            {
                if (city.TryGetComponent(out SpriteRenderer renderer))
                {
                    renderer.color = Color.white;
                }
            }
        }
    }
}