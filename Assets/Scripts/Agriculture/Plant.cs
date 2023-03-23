using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Agriculture
{
    public class Plant : MonoBehaviour
    {
        [SerializeField] private FarmSource farmSource;
        [SerializeField] private Transform[] products;
        [SerializeField] private float maxProductSize;

        public bool _Grothing { get; private set; }
        private bool _canCollect;
        private void Awake()
        {
            for (int i = 0; i < products.Length; i++)
            {
                products[i].localScale = Vector3.zero;
            }
        }
        public virtual void ActivePlant(float firstSize) 
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.one * firstSize;
            for (int i = 0; i < products.Length; i++)
            {
                products[i].localScale = Vector3.zero;
            }
        }
        public virtual void Growth(float maxSize) 
        {
            _Grothing = true;
            transform.DOScale(Vector3.one * maxSize, 1).OnComplete(() => 
            {
                for (int i = 0; i < products.Length; i++)
                {
                    products[i].DOScale(Vector3.one * maxProductSize, 0.1f);
                }
                _canCollect = true;
            });
        }
        public virtual void Collect(float firstSize, float radiusRandomFall)
        {
            if (!_canCollect) return;
            for (int i = 0; i < products.Length; i++)
            {
                var product = Instantiate(products[i], products[i].position, products[i].rotation);
                product.DOJump(GetRandomFallPoint(radiusRandomFall), 1.5f, 1, 0.5f).SetEase(Ease.OutBounce).OnComplete(() => 
                {
                    farmSource.Add(1, product.position);
                    Destroy(product.gameObject);
                });
            }
            _canCollect = false;
            _Grothing = false;
            gameObject.SetActive(false);
        }

        private Vector3 GetRandomFallPoint(float radius) 
        {
            Vector3 point = transform.position;
            point += Random.insideUnitSphere * radius;
            point.y = transform.position.y;
            return point;
        }
    }
}
