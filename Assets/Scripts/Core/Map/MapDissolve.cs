using UnityEngine;
using DG.Tweening;

namespace Core
{
    public class MapDissolve : MonoBehaviour
    {
        [Header("Dissolve")]
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private CircleCollider2D _circleCollider;
        [SerializeField, Min(0f)]
        private float _dissolveDuration;

        private float _colliderStartRadius;
        private Material _material;

        private void Start()
        {
            _colliderStartRadius = _circleCollider.radius;
            _material = _spriteRenderer.material;
            StartDissolve();
        }

        private void SetDissolveValue(float value)
        {
            _material.SetFloat("_DissolveValue", value);
            _circleCollider.radius = Mathf.Lerp(0f, _colliderStartRadius, value);
        }

        public void StartDissolve()
        {
            DOTween.To(() => 1f, x => SetDissolveValue(x), 0f, _dissolveDuration).OnComplete(() =>
            {
                _circleCollider.enabled = false;
            });
        }
    }
}