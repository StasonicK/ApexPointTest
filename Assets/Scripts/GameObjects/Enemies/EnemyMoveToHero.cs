using System;
using UnityEngine;
using UnityEngine.AI;

namespace GameObjects.Enemies
{
    public class EnemyMoveToHero : MonoBehaviour, IGameObjectMovement
    {
        [SerializeField] private NavMeshAgent _agent;

        private Transform _tankTransform;

        public bool IsRun { get; private set; }

        private void Update()
        {
            SetDestinationForAgent();
        }

        public void Construct(Transform tankTransform, float speed)
        {
            _tankTransform = tankTransform;
            _agent.speed = speed;
            _agent.enabled = false;
        }

        private void SetDestinationForAgent()
        {
            if (_tankTransform && _agent != null)
            {
                if (IsRun)
                {
                    try
                    {
                        _agent.SetDestination(_tankTransform.position);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Debug.Log($"SetDestinationForAgent error: {e}");
                        throw;
                    }
                }
            }
        }

        public void Run()
        {
            IsRun = true;
            _agent.enabled = true;
        }

        public void Stop()
        {
            IsRun = false;
            _agent.enabled = false;
        }
    }
}