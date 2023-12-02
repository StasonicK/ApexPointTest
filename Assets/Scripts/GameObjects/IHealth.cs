using System;

namespace GameObjects
{
    public interface IHealth
    {
        void GetHit(float damage);
        event Action HealthChanged;
        float Max { get; }
        float Current { get; }
    }
}