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

        public override void Handle(AvatarModel model) {
            _coroutine = StartCoroutine(LoadAvatar(model));
        }

        private IEnumerator LoadAvatar(AvatarModel model) {
            string src = model.src;
            var handle = RPMRequestHandle.Request(src, model.transform);
            yield return handle;
            
            if (handle.Character) {
                var avatar = handle.Character.GetComponent<Avatar>();
                
                var animator = handle.Character.GetComponent<Animator>();
                animator.runtimeAnimatorController = runtimeAnimatorController;
                    
                Debug.Log($"[RPMAvatarOperation] Loaded {model.src}");
                avatarsPool.AddAvatar(avatar, src);
                model.Complete(avatar);
            } else {
                Debug.Log($"[RPMAvatarOperation] Failed to load {model.src}. Using {defaultKey} instead with addressable avatars.");
                model.src = defaultKey;
                model.type = 0;
                addressableAvatarOperation.Handle(model);
            }
        }

        public override void Stop() {
            StopCoroutine(_coroutine);
        }
    }
}