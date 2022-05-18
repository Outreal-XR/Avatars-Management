using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.outrealxr.avatars
{
    public class RPMAvatarPool : AvatarsPool
    {
        [SerializeField, Range(2, 100)] private int maxRPMAvatarCount = 2;
        private readonly List<Avatar> _avatars = new List<Avatar>();
        
        public override void AddAvatar(Avatar avatar, string src) {
            if (IsPoolMaxed("")) 
                Dispose(_avatars[_avatars.Count - 1]);

            _avatars.Add(avatar);
        }

        public override Avatar GetNextAvailable(string src) {
            return null;
        }

        public override Avatar GetInactive(string src) {
            return null;
        }

        private void Dispose(Avatar avatar) {
            avatar.owner.AvatarRemoved();
            Destroy(avatar.gameObject);
        }

        public override bool IsPoolMaxed(string src) => _avatars.Count >= maxRPMAvatarCount;
        
    }
}