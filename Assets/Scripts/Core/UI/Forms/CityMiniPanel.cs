using UnityEngine;
using Core.Cities;
using System;
using UnityEngine.UI;

namespace Core.UI.Forms
{
    [RequireComponent(typeof(RectTransform))]
    public class CityMiniPanel : MonoBehaviour, IWorldSpaceForm<CityScript>, IClosableForm
    {
        private const string FormPath = "Prefabs/UI/Forms/City Mini-Panel";

        [Header("References")]
        [SerializeField]
        private Button _templeButton;
        [SerializeField, Min(0f)]
        private float _offsetY = 1f;
        private RectTransform _rectTransform;
        public CityScript AttachedTarget { get; private set; }
        public event Action BuildTemple;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _templeButton.onClick.AddListener(OnBuildTemple);
        }
        private void Update()
        {
            if (AttachedTarget == null) return;
            _rectTransform.position = Utils.WorldToScreenPoint(AttachedTarget.transform.position + Vector3.up * _offsetY);
        }
        private void OnDestroy()
        {
            _templeButton.onClick.RemoveListener(OnBuildTemple);
        }
        private void OnBuildTemple()
        {
            BuildTemple?.Invoke();
            Close();
        }
        public static IWorldSpaceForm<CityScript> CreateForm(CityScript target)
        {
            var obj = Instantiate(Resources.Load(FormPath)) as GameObject;
            var form = obj.GetComponentInChildren<CityMiniPanel>();
            form.AttachTo(target);
            return form;
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