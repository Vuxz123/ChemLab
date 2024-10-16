namespace com.ethnicthv.chemlab.engine.api.context.properties
{
    public interface IPressure : IContextProperty<float>
    {
        public float GetPressure();
        public float GetPressureInAtmospheres();
        public float GetPressureInPascals();
        public float GetPressureInKilopascals();
        public float GetPressureInMillimetersOfMercury();
    }
}