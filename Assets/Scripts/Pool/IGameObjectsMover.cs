namespace Pool
{
    public interface IGameObjectsMover
    {
        void Construct(IEnemiesGenerator enemiesGenerator);
        void Run();
        void Stop();
    }
}