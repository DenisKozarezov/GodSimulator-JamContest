using UnityEngine;
using Core.Cities;

namespace Core.UI.Forms
{
    [RequireComponent(typeof(RectTransform))]
    public class CityMiniPanel : MonoBehaviour, IWorldSpaceForm<CityScript>, IClosableForm
    {
        [Header("Form")]
        [SerializeField, Min(0f)]
        private float _offsetY = 1f;
        private RectTransform _rectTransform;
        public CityScript AttachedTarget { get; private set; }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        private void Update()
        {
            if (AttachedTarget == null) return;
            _rectTransform.position = Utils.WorldToScreenPoint(AttachedTarget.transform.position + Vector3.up * _offsetY);
        }

        public void AttachTo(CityScript target)
        {
            AttachedTarget = target;
        }
        public void Close()
        {
            Destroy(transform.parent.gameObject);
        }
    }
}