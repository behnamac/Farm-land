using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Player
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private float speedMove;
        [SerializeField] private float speedRotate;

        private float _angleVelocity;
        private AnimationController _animationController;

        private void Awake()
        {
            TryGetComponent(out _animationController);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Movement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        private void Movement(float horizontal, float vertical)
        {
            Vector3 Dirction = new Vector3(horizontal, 0, vertical);

            // Rotate
            if (Dirction.magnitude > 0.1f)
            {
                Transform camera = Camera.main.transform;
                float targetAngle = Mathf.Atan2(Dirction.x, Dirction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _angleVelocity, speedRotate);
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }

            // Move
            float move = Mathf.Max(Mathf.Abs(Dirction.x), Mathf.Abs(Dirction.z));
            transform.Translate(Vector3.forward * (move * speedMove) * Time.deltaTime);
            _animationController.MoveAnimation(move);
        }
    }
}
