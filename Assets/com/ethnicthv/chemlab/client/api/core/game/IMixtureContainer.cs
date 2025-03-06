using com.ethnicthv.chemlab.engine.mixture;

namespace com.ethnicthv.chemlab.client.api.core.game
{
    public interface IMixtureContainer
    {
        public float GetVolumn();
        public void SetVolumn(float volumn);
        public Mixture GetMixture();
        public void SetMixture(Mixture mixture);
        public void SetMixtureAndVolumn(Mixture mixture, float volumn);
        public (Mixture mixture, float volumn) GetMixtureAndVolumn();
    }
}