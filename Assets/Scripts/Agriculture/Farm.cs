using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agriculture
{
    public enum FarmState 
    {
        SowingSeeds,
        Irrigation,
        Reap
    }
    public class Farm : MonoBehaviour
    {
        public Plant[] plants;
        [SerializeField] protected float distanceRayCheck;
        [SerializeField] protected float growthingTime;
        [SerializeField] protected float firstSize = 0.3f;
        [SerializeField] protected float maxSizePlant = 1;
        [SerializeField] protected bool activeOnAwake;

        public FarmState Farm_State { get; private set; }
        public bool ActiveFarm { get; set; }
        protected virtual void Awake()
        {
            for (int i = 0; i < plants.Length; i++)
            {
                plants[i].transform.localScale = Vector3.one * firstSize;
                plants[i].gameObject.SetActive(false);
            }

            ActiveFarm = activeOnAwake;
        }
        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            for (int i = 0; i < plants.Length; i++)
            {
                Gizmos.DrawWireSphere(plants[i].transform.position, distanceRayCheck);
            }
        }

        public virtual void SendSeed(Vector3 rayPoint) 
        {
            List<GameObject> activePlants = new List<GameObject>();
            for (int i = 0; i < plants.Length; i++)
            {
                bool distance = CheckDistance(rayPoint, plants[i].transform.position, distanceRayCheck);
                if (distance && !plants[i].gameObject.activeInHierarchy) 
                {
                    plants[i].ActivePlant(firstSize);
                }
                if (plants[i].gameObject.activeInHierarchy)
                    activePlants.Add(plants[i].gameObject);
            }

            float precent = ((float)activePlants.Count / (float)plants.Length) * 100;
            if (precent >= 60) 
            {
                for (int i = 0; i < plants.Length; i++)
                {
                    plants[i].ActivePlant(firstSize);
                }
                ChangeState(FarmState.Irrigation);
            }
        }
        public virtual void SendWater(Vector3 rayPoint)
        {
            List<Plant> activePlants = new List<Plant>();
            for (int i = 0; i < plants.Length; i++)
            {
                bool distance = CheckDistance(rayPoint, plants[i].transform.position, distanceRayCheck);
                if (distance && !plants[i]._Grothing)
                {
                    plants[i].Growth(maxSizePlant);
                }
                if (plants[i]._Grothing)
                    activePlants.Add(plants[i]);
            }

            float precent = ((float)activePlants.Count / (float)plants.Length) * 100;
            if (precent >= 60)
            {
                for (int i = 0; i < plants.Length; i++)
                {
                    plants[i].Growth(maxSizePlant);
                }
                //ChangeState(FarmState.Reap);
                StartCoroutine(ChangeStateDelay(FarmState.Reap));
            }
        }
        public virtual void Reap(Vector3 rayPoint) 
        {
            List<Plant> activePlants = new List<Plant>();
            for (int i = 0; i < plants.Length; i++)
            {
                bool distance = CheckDistance(rayPoint, plants[i].transform.position, distanceRayCheck);
                if (distance && plants[i].gameObject.activeInHierarchy)
                {
                    plants[i].Collect(firstSize, distanceRayCheck);
                }
                if (!plants[i].gameObject.activeInHierarchy)
                    activePlants.Add(plants[i]);
            }

            float precent = ((float)activePlants.Count / (float)plants.Length) * 100;
            if (precent >= 60)
            {
                for (int i = 0; i < plants.Length; i++)
                {
                    if (plants[i].gameObject.activeInHierarchy)
                        plants[i].Collect(firstSize, distanceRayCheck);
                }
                ChangeState(FarmState.SowingSeeds);
            }
        }

        public virtual void ChangeState(FarmState state) 
        {
            Farm_State = state;
        }

        private IEnumerator ChangeStateDelay(FarmState state) 
        {
            yield return new WaitForSeconds(2);
            Farm_State = state;
        }

        private bool CheckDistance(Vector3 pointA, Vector3 pointB, float targetDistance) 
        {
            pointA.y = 0;
            pointB.y = 0;
            float distance = Vector3.Distance(pointA, pointB);
            if (distance <= targetDistance)
                return true;

            return false;
        }
    }
}
