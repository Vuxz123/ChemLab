namespace com.ethnicthv.chemlab.engine.api.context.properties.provider
{
    public interface IPressureProvider<P> where P : IPressure
    {
        public P GetPressure();
    }
}