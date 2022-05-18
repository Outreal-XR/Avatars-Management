using System.Collections;
using ReadyPlayerMe;
using UnityEngine;

namespace com.outrealxr.avatars
{
    public class RPMAvatarOperation : AvatarLoadingOperation
    {
        public string defaultKey = "yBot";
        private Coroutine _coroutine;
        
        private void Awake()
        {
            avatarsPool = GetComponent<RPMAvatarPool>();
        }

        public override void Handle(AvatarModel model) {
            _coroutine = StartCoroutine(LoadAvatar(model));
        }

        private IEnumerator LoadAvatar(AvatarModel model) {
            string src = model.src;
            var handle = RPMRequestHandle.Request(src);
            yield return handle;
            
            if (handle.Character) {
                var avatar = handle.Character.AddComponent<Avatar>();
                Debug.Log($"[AddressableAvatarOperation] Loaded {model.src}");
                avatarsPool.AddAvatar(avatar, src);
                model.Complete(avatar);
            } else {
                Debug.Log($"[AddressableAvatarOperation] Failed to load {model.src}. Using {defaultKey} instead");
                model.src = defaultKey;
                Handle(model);
            }
        }

        public override void Stop() {
            StopCoroutine(_coroutine);
        }
    }
}