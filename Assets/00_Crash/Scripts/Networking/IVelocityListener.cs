using ObliqueSenastions.ClapSpace;
using ObliqueSenastions.VRRigSpace;

namespace ObliqueSenastions.UISpace
{
    public interface IVelocityListener
    {
        void AddVelocityContributor(SimpleVelocityTracker[] velocityContributors);

        //void AddLoadingBarContributor(LoadingBar[] loadingBarContributor);
        
    }

}
