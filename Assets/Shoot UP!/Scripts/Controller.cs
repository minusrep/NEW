namespace Core
{
    public abstract class Controller 
    {
        public abstract bool isInitialized { get; }

        public abstract void Initialize<T>(T model) where T : Model;

        public virtual void Update()
        {

        }
    }
}