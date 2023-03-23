using Agriculture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class CharacterAgriculture : MonoBehaviour
    {
        [SerializeField] protected Transform waterRayPoint;
        [SerializeField] protected ParticleSystem waterParticle;

        protected AnimationController animationController;

        protected virtual void Awake()
        {
            TryGetComponent(out animationController);
        }
        protected virtual void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out Farm farm)) 
            {
                if (!farm.ActiveFarm) return;
                switch (farm.Farm_State)
                {
                    case FarmState.SowingSeeds:
                        SowingSeeds(farm);
                        break;
                    case FarmState.Irrigation:
                        Irrigation(farm);
                        break;
                    case FarmState.Reap:
                        Reap(farm);
                        break;
                }
            }
        }
        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Farm farm))
            {
                if (waterParticle.isPlaying)
                { 
                    waterParticle.Stop();
                    animationController.InactivePissAnimation();
                }
            }
        }
        protected virtual void SowingSeeds(Farm farm) 
        {
            Vector3 hitPoint;
            bool hasHit;
            CheckRaycast(out hitPoint, out hasHit);

            if (!hasHit) return;
            farm.SendSeed(hitPoint);
            if (waterParticle.isPlaying)
            { 
                waterParticle.Stop();
                animationController.InactivePissAnimation();
            }
        }
        protected virtual void Irrigation(Farm farm) 
        {
            Vector3 hitPoint;
            bool hasHit;
            CheckRaycast(out hitPoint, out hasHit);

            if (!hasHit) return;
            farm.SendWater(hitPoint);
            if (!waterParticle.isPlaying)
            { 
                waterParticle.Play();
                animationController.ActivePissAnimation();
            }
        }
        protected virtual void Reap(Farm farm) 
        {
            Vector3 hitPoint;
            bool hasHit;
            CheckRaycast(out hitPoint, out hasHit);

            if (!hasHit) return;
            farm.Reap(hitPoint);
            if (waterParticle.isPlaying)
            { 
                waterParticle.Stop();
                animationController.InactivePissAnimation();
            }
        }

        protected virtual void CheckRaycast(out Vector3 hitPoint, out bool hasHit) 
        {
            RaycastHit hit;
            if (Physics.Raycast(waterRayPoint.position, Vector3.down * 20, out hit))
            {
                if (hit.collider.gameObject.GetComponent<Farm>())
                {
                    hasHit = true;
                    hitPoint = hit.point;
                }
                else
                {
                    hasHit = false;
                    hitPoint = Vector3.zero;
                }
            }
            else
            {
                hasHit = false;
                hitPoint = Vector3.zero;
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(waterRayPoint.position, Vector3.down * 20);
        }
    }
}
