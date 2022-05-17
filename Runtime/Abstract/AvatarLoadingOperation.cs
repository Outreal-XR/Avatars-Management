using UnityEngine;

namespace com.outrealxr.avatars
{
    public abstract class AvatarLoadingOperation : MonoBehaviour
    {
        public AvatarsPool avatarsPool;
        public abstract void Handle(AvatarModel model);
        public abstract void Stop();
    }
}