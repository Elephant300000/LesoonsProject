using UnityEngine;

namespace Lucky38.MyCamera
{
    public interface IPlayerCinemaCamera
    {
        void SetLoocTarget(Transform trTarget);
        void SetLookEnabled(bool enabled);
    }
}
