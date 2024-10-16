namespace com.ethnicthv.chemlab.engine.api.context
{
    public interface IContextProvider<CT> where CT : IContext
    {
        CT GetContext();
    }
}