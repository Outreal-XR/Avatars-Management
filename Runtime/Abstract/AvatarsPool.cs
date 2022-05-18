using System.Collections.Generic;
using UnityEngine;

namespace com.outrealxr.avatars
{
    public abstract class AvatarsPool : MonoBehaviour
    {
        public abstract void AddAvatar(Avatar avatar, string src);

        public abstract Avatar GetNextAvailable(string src);

        public abstract Avatar GetInactive(string src);

        public abstract bool IsPoolMaxed(string src);
    }
}