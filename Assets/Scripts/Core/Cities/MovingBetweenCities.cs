using System.Threading.Tasks;
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
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(playerWantToMovingPriestsSignal.City.transform.position, playerWantToMovingPriestsSignal.TempleRange, Constants.CitiesLayer);
            Collider2D selfCollider = playerWantToMovingPriestsSignal.City.GetComponent<Collider2D>();
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

        public async void Move(PlayerMovingPriestsSignal playerMovingPriestsSignal)
        {
            playerMovingPriestsSignal.FromCity.DeletePriests(playerMovingPriestsSignal.NumberOfPriests);
            await Task.Delay(5000);
            playerMovingPriestsSignal.ToCity.AddPriests(playerMovingPriestsSignal.NumberOfPriests);
        }
    }
}