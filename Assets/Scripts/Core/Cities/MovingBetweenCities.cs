using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core.Infrastructure;
using Zenject;

namespace Core.Cities
{
    public class MovingBetweenCities
    {
        private IEnumerable<Collider2D> _colliders;

        public void ShowRange(TempleDragBeginSignal signal)
        {
            if (_colliders != null && _colliders.Count() != 0)
            {
                DeselectCities();
            }
            Debug.Log("Range " + signal.Temple.Range + "!");
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(signal.Temple.transform.position, signal.Temple.Range, Constants.CitiesLayer);
            Collider2D selfCollider = signal.Temple.GetComponent<Collider2D>();
            _colliders = from collider in colliderArray 
                         where collider != selfCollider 
                         select collider;
            SelectCities(_colliders);
        }
        private void SelectCities(IEnumerable<Collider2D> colliders)
        {
            foreach(var collider in colliders)
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