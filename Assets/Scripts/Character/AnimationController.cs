using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class AnimationController : MonoBehaviour
    {
        protected Animator Anim;

        protected virtual void Awake()
        {
            TryGetComponent(out Anim);
        }

        public virtual void MoveAnimation(float value)
        {
            Anim.SetFloat("Move", value);
        }
        public virtual void ActivePissAnimation()
        {
            Anim.SetBool("Piss", true);
        }
        public virtual void InactivePissAnimation()
        {
            Anim.SetBool("Piss", false);
        }
    }
}
