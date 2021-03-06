using UnityEngine;
using Unity.Mathematics;

namespace Core.UI
{
    public class PranaView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _renderer;

        private Material _material;
        private float ArcMin;
        private float ArcMax;

        public Color color
        {
            get => _material.color;
            set => SetColor(value);
        }

        private void Start()
        {
            _material = _renderer.material;
            ArcMin = _material.GetFloat("_Arc1");
            ArcMax = 360f - _material.GetFloat("_Arc2");
        }
        private void SetColor(Color color)
        {
            _material.SetColor("_Color", color);
        }
        public void SetFillAmount(float fillAmount)
        {
            _material.SetFloat("_Arc1", math.lerp(ArcMin, ArcMax, fillAmount));
        }
    }
}