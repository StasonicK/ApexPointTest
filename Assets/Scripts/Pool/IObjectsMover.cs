namespace Pool
{
    public interface IObjectsMover
    {
        void Construct(IObjectsGenerator objectsGenerator);
        void Run();
        void Stop();
    }
}