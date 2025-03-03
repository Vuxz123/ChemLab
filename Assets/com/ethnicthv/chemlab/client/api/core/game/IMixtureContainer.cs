using com.ethnicthv.chemlab.engine.mixture;

namespace com.ethnicthv.chemlab.client.api.core.game
{
    public interface IMixtureContainer
    {
        public Mixture GetMixture();
        public void SetMixture(Mixture mixture);
    }
}