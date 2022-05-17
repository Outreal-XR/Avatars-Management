using UnityEngine;

namespace com.outrealxr.avatars
{
    public abstract class AvatarLocalController : MonoBehaviour
    {
        public static AvatarLocalController instance;

        private void Awake()
        {
            instance = this;
        }

        public abstract void UpdateLocalModel(string src);
    }
}