using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Storage;
using Unity.VisualScripting;

namespace Agriculture
{
    public class FarmSource : MonoBehaviour
    {
        [SerializeField] protected string sourceName;
        [SerializeField] protected int firstValue;
        [SerializeField] protected TextMeshProUGUI counterText;
        [SerializeField] protected Transform productImagePrefab;
        [SerializeField] protected Transform targetProductImage;
        [SerializeField] protected Transform canvas;

        private int _currentValue;
        public int CurrenValue => _currentValue;

        protected virtual void Awake()
        {
            FarmSourceData farmSourceData = JSONSaver.LoadFarmSourceData(sourceName);
            if (farmSourceData != null)
                _currentValue = farmSourceData.Value;
            else 
            {
                _currentValue = firstValue;
                JSONSaver.SaveFarmSourceData(sourceName, _currentValue);
            }

            UpdateText();
        }
        public virtual void Add(int value, Vector3 spawnPoint)
        {
            _currentValue += value;
            Vector3 genaratePoint = Camera.main.WorldToScreenPoint(spawnPoint);
            for (int i = 0; i < value; i++)
            {
                var productImage = Instantiate(productImagePrefab, genaratePoint, Quaternion.identity, canvas);
                productImage.transform.localScale = Vector3.one;
                productImage.DOMove(targetProductImage.position, 0.3f);
                Destroy(productImage.gameObject, 0.3f);
            }

            JSONSaver.SaveFarmSourceData(sourceName, CurrenValue);
            UpdateText();
        }
        public virtual void Remove(int value)
        {
            _currentValue -= value;
            JSONSaver.SaveFarmSourceData(sourceName, CurrenValue);
            UpdateText();
        }

        private void UpdateText() 
        {
            counterText.text = CurrenValue.ToString();
        }
    }
}
