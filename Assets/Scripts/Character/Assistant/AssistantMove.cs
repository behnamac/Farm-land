using Agriculture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Character.Assistant
{
    public class AssistantMove : MonoBehaviour
    {
        [SerializeField] private Farm targetFarm;
        [SerializeField] private float delayMove = 0;
        [SerializeField] private bool activeOnAwake;

        private bool _activeMove;
        private Transform _targetMove;
        private NavMeshAgent _navMesh;
        private AnimationController _animationController;
        private void Awake()
        {
            TryGetComponent(out _navMesh);
            TryGetComponent(out _animationController);

            if (activeOnAwake)
                ActiveAssistant();
        }

        private void Update()
        {
            _animationController.MoveAnimation(_navMesh.velocity.magnitude);
            if (!_targetMove) return;
            CalculateDistance();
        }

        public void ActiveAssistant()
        {
            _activeMove = true;

            Move();
        }

        private void Move()
        {
            _targetMove = GetRandomTargetMove();
            _navMesh.SetDestination(_targetMove.position);
        }

        private void CalculateDistance()
        {
            Vector3 targetPoint = _targetMove.position;
            targetPoint.y = transform.position.y;
            float distance = Vector3.Distance(transform.position, targetPoint);
            if (distance <= _navMesh.stoppingDistance)
            {
                _targetMove = null;
                Invoke(nameof(Move), delayMove);
            }
        }

        private Transform GetRandomTargetMove()
        {
            Plant[] targets = targetFarm.plants;
            int randomIndex = Random.Range(0, targets.Length);
            Transform target = targets[randomIndex].transform;
            return target;
        }
    }
}
