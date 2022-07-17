using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core.Infrastructure;

namespace Core.Cities
{
    public class MovingBetweenCities
    {
        private List<Collider2D> colliders;

        public void ShowRange(PlayerWantToMovingPriestsSignal playerWantToMovingPriestsSignal)
        {
            if (colliders != null && colliders.Count != 0)
            {
                DeselectCities();
            }
            Debug.Log("Range " + playerWantToMovingPriestsSignal.TempleRange + "!");
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(playerWantToMovingPriestsSignal.City.transform.position, playerWantToMovingPriestsSignal.TempleRange);
            Collider2D selfCollider = playerWantToMovingPriestsSignal.City.GetComponent<Collider2D>();
            colliders = colliderArray.ToList();
            colliders.Remove(selfCollider);
            SelectCities(colliders);
        }

        private void SelectCities(IEnumerable<Collider2D> colliders)
        {
            foreach(var collider in colliders)
            {
                if (collider.TryGetComponent<GreeceCityScript>(out GreeceCityScript city))
                {
                    city.GetComponent<SpriteRenderer>().color = Color.green;
                }
            }
        }

        private void DeselectCities()
        {
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<GreeceCityScript>(out GreeceCityScript city))
                {
                    city.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }

        public async void Move(PlayerMovingPriestsSignal playerMovingPriestsSignal)
        {
            playerMovingPriestsSignal.FromCity.DeletePriests(playerMovingPriestsSignal.God, playerMovingPriestsSignal.NumberOfPriests);
            await Task.Delay(5000);
            playerMovingPriestsSignal.ToCity.AddPriests(playerMovingPriestsSignal.God, playerMovingPriestsSignal.NumberOfPriests);
        }
    }
}