using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.outrealxr.avatars
{
    public class AddressableAvatarPool : AvatarsPool
    {
        [Range(2, 100)]
        public int poolSizePerLabel = 2;
        public Dictionary<string, int> nextAvatar = new Dictionary<string, int>();
        public Dictionary<string, Avatar[]> avatars = new Dictionary<string, Avatar[]>();

        public override void AddAvatar(Avatar avatar, string src)
        {
            if (!avatars.ContainsKey(src)) {
                avatars.Add(src, new Avatar[poolSizePerLabel]);
                nextAvatar.Add(src, 0);
            }
            
            for (int i = 0; i < avatars[src].Length; i++)
                if (avatars[src][i] == null)
                {
                    avatars[src][i] = avatar;
                    return;
                }
        }

        public override Avatar GetNextAvailable(string src)
        {
            Avatar avatar = avatars[src][nextAvatar[src]];
            nextAvatar[src] = (nextAvatar[src] + 1) % poolSizePerLabel;
            return (avatar.owner == null || !avatar.owner.isLocal) ? avatar : GetNextAvailable(src);
        }

        public override Avatar GetInactive(string src)
        {
            if (avatars.ContainsKey(src))
                for (int i = 0; i < avatars[src].Length; i++)
                    if (avatars[src][i] != null && !avatars[src][i].gameObject.activeInHierarchy)
                        return avatars[src][i];
            return null;
        }

        public override bool IsPoolMaxed(string src)
        {
            if (!avatars.ContainsKey(src)) return false;
            for (int i = 0; i < avatars[src].Length; i++) if (avatars[src][i] == null) return false;
            return true;
        }
    }
}