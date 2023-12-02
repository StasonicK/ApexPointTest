using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace GameObjects.Enemies
{
    public class EnemyMovement : MonoBehaviour, IGameObjectMovement
    {
        [SerializeField] private NavMeshAgent _agent;

        private Transform _tankTransform;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.5f);

        public bool IsRun { get; private set; }

        private void OnEnable()
        {
            StartCoroutine(CoroutineDelay());
        }

        private void OnDisable()
        {
            Stop();
        }

        private IEnumerator CoroutineDelay()
        {
            if (IsRun)
                Stop();

            yield return _waitForSeconds;
            Run();
        }

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