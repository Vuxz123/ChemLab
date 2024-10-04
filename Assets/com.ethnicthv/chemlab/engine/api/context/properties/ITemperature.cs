namespace com.ethnicthv.chemlab.engine.api.context.properties
{
    public interface ITemperature : IContextProperty<float>
    {
        public float GetTemperature();
        public float GetTemperatureInCelsius();
        public float GetTemperatureInFahrenheit();
        public float GetTemperatureInKelvin();
    }
}