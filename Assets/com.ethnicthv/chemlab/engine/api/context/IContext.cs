namespace com.ethnicthv.chemlab.engine.api.context
{
    public interface IContext
    {
        public T GetProperty<T>(IContextProperty<T> property);
    }
}