namespace Sources.Infrastructure
{
    public interface IDelayedUpdatable
    {
        void DelayedTick(float deltaTime);
    }
}