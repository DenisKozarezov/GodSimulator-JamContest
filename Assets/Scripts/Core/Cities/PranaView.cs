using UnityEngine;

namespace Core.Cities
{
    public class PranaView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _renderer;

        private Material _material;
        private float ArcMin;
        private float ArcMax => 360 - ArcMin / 2;

        private void Start()
        {
            _material = _renderer.material;
            ArcMin = _renderer.material.GetFloat("_Arc1");
        }
        public void SetFillAmount(float fillAmount)
        {
            _material.SetFloat("_Arc1", Mathf.Lerp(ArcMin, ArcMax, fillAmount));
        }
    }
}