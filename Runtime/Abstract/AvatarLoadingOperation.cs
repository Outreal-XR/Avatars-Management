using UnityEngine;

namespace com.outrealxr.avatars
{
    public abstract class AvatarLoadingOperation : MonoBehaviour
    {
        public AvatarsPool avatarsPool;
        public abstract void Handle(AvatarModel model, string src);
        public abstract void Stop();
    }
}