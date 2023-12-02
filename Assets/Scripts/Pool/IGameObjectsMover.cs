namespace Pool
{
    public interface IGameObjectsMover
    {
        void Construct(IGameObjectsGenerator gameObjectsGenerator);
        void Run();
        void Stop();
    }
}