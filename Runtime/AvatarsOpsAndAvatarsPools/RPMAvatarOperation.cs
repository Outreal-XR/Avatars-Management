using System.Collections;
using ReadyPlayerMe;
using UnityEngine;

namespace com.outrealxr.avatars
{
    public class RPMAvatarOperation : AvatarLoadingOperation
    {
        [SerializeField] private string defaultKey = "yBot";
        private Coroutine _coroutine;

        [SerializeField] private RuntimeAnimatorController runtimeAnimatorController;
        [SerializeField] private AddressableAvatarOperation addressableAvatarOperation;

        
        private void Awake()
        {
            avatarsPool = GetComponent<RPMAvatarPool>();
        }

        public override void Handle(AvatarModel model, string src) {
            _coroutine = StartCoroutine(LoadAvatar(model, src));
        }

        private IEnumerator LoadAvatar(AvatarModel model, string src) {
            var handle = RPMRequestHandle.Request(src, model.transform);
            yield return handle;
            
            if (handle.Character) {
                var avatar = handle.Character.GetComponent<Avatar>();
                avatar.type = AvatarsProvider.instance.avatarLoadingOperations.IndexOf(this);
                
                var animator = handle.Character.GetComponent<Animator>();
                animator.runtimeAnimatorController = runtimeAnimatorController;
                    
                Debug.Log($"[RPMAvatarOperation] Loaded {model.src}");
                avatarsPool.AddAvatar(avatar, src);
                model.Complete(avatar);
            } else {
                Debug.Log($"[RPMAvatarOperation] Failed to load {model.src}. Using {defaultKey} instead with addressable avatars.");
                addressableAvatarOperation.Handle(model, defaultKey);
            }
        }

        public override void Stop() {
            StopCoroutine(_coroutine);
        }
    }
}