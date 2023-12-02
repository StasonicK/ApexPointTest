namespace GameObjects
{
    public interface IGameObjectMovement
    {
        bool IsRun { get; }

        public void Run();
        public void Stop();
    }
}