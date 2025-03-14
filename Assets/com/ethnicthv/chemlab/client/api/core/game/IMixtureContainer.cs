using com.ethnicthv.chemlab.engine.mixture;

namespace com.ethnicthv.chemlab.client.api.core.game
{
    public interface IMixtureContainer
    {
        public float GetVolume();
        public void SetVolume(float volume);
        public Mixture GetMixture();
        public void SetMixture(Mixture mixture);
        public void SetMixtureAndVolume(Mixture mixture, float volume);
        public (Mixture mixture, float volume) GetMixtureAndVolume();
        public bool IsEmpty();
        public void Clear();
    }
}