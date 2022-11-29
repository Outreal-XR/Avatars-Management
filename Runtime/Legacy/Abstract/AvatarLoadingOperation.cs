using UnityEngine;

namespace com.outrealxr.avatars.revised
{
    public abstract class AvatarLoadingOperation : MonoBehaviour
    {
        public bool running;
        public abstract void Handle(AvatarModel model);
    }
}