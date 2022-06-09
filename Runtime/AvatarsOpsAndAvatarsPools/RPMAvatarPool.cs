using System.Collections;
using System.Collections.Generic;
using GLTFast;
using UnityEngine;

namespace com.outrealxr.avatars
{
    public class RPMAvatarPool : AvatarsPool
    {
        public const string GltfHolderName = "GLTF Holder";
        
        [SerializeField, Range(2, 100)] private int maxRPMAvatarCount = 2;
        private readonly List<Avatar> _avatars = new();
        
        public override void AddAvatar(Avatar avatar, string src) {
            if (IsPoolMaxed("")) {
                Dispose(_avatars[0]);
                _avatars.RemoveAt(0);
            }

            _avatars.Add(avatar);
        }

        public override Avatar GetNextAvailable(string src) {
            return null;
        }

        public override Avatar GetInactive(string src) {
            return null;
        }

        private void Dispose(Avatar avatar) {
            if (!avatar) return;
            if (avatar.owner) avatar.owner.AvatarRemoved();
            
            var gltfHolder = avatar.transform.parent.Find(GltfHolderName);
            gltfHolder.GetComponent<GltfAsset>().Dispose();
            Destroy(gltfHolder.gameObject);
            
            Destroy(avatar.gameObject);
        }

        public override bool IsPoolMaxed(string src) => _avatars.Count >= maxRPMAvatarCount;
        
    }
}