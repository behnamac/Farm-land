using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Agriculture;
using DG.Tweening;
using Unity.VisualScripting;
using Character.Player;
using Storage;
using TMPro;

namespace Lock
{
    public class LockSystem : MonoBehaviour
    {
        [SerializeField] private string lockName;
        [SerializeField] private Image timerImage;
        [SerializeField] private SourceHolder[] sourceHolders;
        [SerializeField] private float buyTime;
        [SerializeField] private GameObject[] activeObjects;
        [SerializeField] private GameObject[] inactiveObjects;
        public UnityEvent OnBuy;

        private bool _enter;
        private void Start()
        {
            LockData lockData = JSONSaver.LoadLockData(lockName);
            if (lockData != null) 
            {
                if (lockData.Value)
                    Unlock();
            }

            for (int i = 0; i < sourceHolders.Length; i++)
            {
                sourceHolders[i].valueText.text = sourceHolders[i].needValue.ToString();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerMove player))
            {
                _enter = true;
                if (!CheckSource()) return;
                timerImage.fillAmount = 1;
                timerImage.DOFillAmount(0, buyTime).OnComplete(() =>
                {
                    if (_enter)
                    {
                        Unlock();
                        for (int i = 0; i < sourceHolders.Length; i++)
                            sourceHolders[i].needSource.Remove(sourceHolders[i].needValue);
                        JSONSaver.SaveLockData(lockName, true);
                    }
                });
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerMove player))
            {
                _enter = false;
                timerImage.DOPause();
                timerImage.fillAmount = 1;
            }
        }

        private void Unlock()
        {
            for (int i = 0; i < activeObjects.Length; i++)
                activeObjects[i].SetActive(true);

            for (int i = 0; i < inactiveObjects.Length; i++)
                inactiveObjects[i].SetActive(false);

            OnBuy?.Invoke();

            Destroy(this);
        }
        private bool CheckSource()
        {
            for (int i = 0; i < sourceHolders.Length; i++)
            {
                if (sourceHolders[i].needSource.CurrenValue < sourceHolders[i].needValue)
                    return false;
            }
            return true;
        }

        [System.Serializable]
        private class SourceHolder
        {
            public FarmSource needSource;
            public int needValue;
            public TextMeshProUGUI valueText;
        }
    }
}
