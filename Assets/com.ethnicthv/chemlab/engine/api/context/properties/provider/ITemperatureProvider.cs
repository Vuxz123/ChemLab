namespace com.ethnicthv.chemlab.engine.api.context.properties.provider
{
    public interface ITemperatureProvider<P> where P : ITemperature
    {
        public P GetTemperature();
    }
}