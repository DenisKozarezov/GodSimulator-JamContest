using UnityEngine;
using Unity.Mathematics;

namespace Core.Cities
{
    public class PranaView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _renderer;

        private Material _material;
        private float ArcMin;
        private float ArcMax => 360f - ArcMin / 2f;

        private void Start()
        {
            _material = _renderer.material;
            ArcMin = _renderer.material.GetFloat("_Arc1");
        }
        public void SetFillAmount(float fillAmount)
        {
            _material.SetFloat("_Arc1", math.lerp(ArcMin, ArcMax, fillAmount));
        }
    }
}