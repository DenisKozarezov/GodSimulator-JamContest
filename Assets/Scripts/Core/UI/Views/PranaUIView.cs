using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

namespace Core.Cities
{
    public class PranaUIView : MonoBehaviour
    {
        [SerializeField]
        private Image _renderer;

        private Material _material;
        private float ArcMin;
        private float ArcMax;

        private void Start()
        {
            _renderer.material = new Material(_renderer.material);
            _material = _renderer.material;
            ArcMin = _material.GetFloat("_Arc1");
            ArcMax = 360f - _material.GetFloat("_Arc2");
        }
        public void SetFillAmount(float fillAmount)
        {
            _material.SetFloat("_Arc1", math.lerp(ArcMin, ArcMax, fillAmount));
        }
        public void SetColor(Color color)
        {
            _material.SetColor("_Color", color);
        }
    }
}