using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core.Infrastructure;

namespace Core.Cities
{
    public class MovingBetweenCities
    {
        private IEnumerable<Collider2D> _colliders;

        public void ShowRange(PlayerWantToMovingPriestsSignal playerWantToMovingPriestsSignal)
        {
            if (_colliders != null && _colliders.Count() != 0)
            {
                DeselectCities();
            }
            Debug.Log("Range " + playerWantToMovingPriestsSignal.TempleRange + "!");
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(playerWantToMovingPriestsSignal.Transform.position, playerWantToMovingPriestsSignal.TempleRange, Constants.CitiesLayer);
            Collider2D selfCollider = playerWantToMovingPriestsSignal.Transform.GetComponent<Collider2D>();
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