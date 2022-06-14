using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace com.outrealxr.avatars
{
    [RequireComponent(typeof(AddressableAvatarPool))]
    public class AddressableAvatarOperation : AvatarLoadingOperation
    {
        public string defaultKey = "yBot";
        Coroutine coroutine;
        
        private void Awake()
        {
            avatarsPool = GetComponent<AddressableAvatarPool>();
        }

        public override void Handle(AvatarModel model, string src)
        {
            Avatar avatar = avatarsPool.GetInactive(src);
            if (avatar != null) model.Complete(avatar);
            else if (avatarsPool.IsPoolMaxed(src)) model.Complete(avatarsPool.GetNextAvailable(src));
            else coroutine = StartCoroutine(Download(model, src));
        }

        private IEnumerator Download(AvatarModel model, string src) {
            Avatar avatar;
            AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(src);
            yield return locationsHandle;
            AsyncOperationHandle<GameObject> handle;
            if (locationsHandle.Result.Count > 0)
            {
                handle = Addressables.InstantiateAsync(src);
                yield return handle;
                avatar = handle.Result.GetComponent<Avatar>();
                avatar.type = AvatarsProvider.instance.avatarLoadingOperations.IndexOf(this);
                Debug.Log($"[AddressableAvatarOperation] Loaded {src}");
                avatarsPool.AddAvatar(avatar, src);
                model.Complete(avatar);
            }
            else
            {
                Debug.Log($"[AddressableAvatarOperation] Failed to load {src}. Using {defaultKey} instead with addressable avatars.");
                Handle(model, defaultKey);
            }
            
        }

        public override void Stop()
        {
            StopCoroutine(coroutine);
        }
    }
}