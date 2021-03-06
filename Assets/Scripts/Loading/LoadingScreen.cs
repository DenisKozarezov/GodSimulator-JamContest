using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Core.Loading
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField]
        private Image _progressBar;
        [SerializeField]
        private TextMeshProUGUI _description;
        [SerializeField, Range(0f, 1f)]
        private float _progressBarSpeed;

        private Tweener _tweener;

        public async Task LoadAndDestroyAsync(Queue<LazyLoadingOperation> operations)
        {
            foreach (var operation in operations)
            {
                ILoadingOperation value = operation.Value;

                ResetFill();
                _description.text = value.Description;
                await value.AwaitForLoad(OnProgress);
                await Task.Delay(TimeSpan.FromSeconds(1f));
            }
            Destroy(gameObject);
        }
        private void OnProgress(float value)
        {
            _tweener?.Kill();
            _tweener = _progressBar.DOFillAmount(value, _progressBarSpeed);
        }
        private void ResetFill()
        {
            _tweener?.Kill();
            _progressBar.fillAmount = 0;
        }
    }
}