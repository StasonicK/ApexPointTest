namespace Pool
{
    public interface IGenerator
    {
        void Reset();
        void On();
        void Off();
    }
}