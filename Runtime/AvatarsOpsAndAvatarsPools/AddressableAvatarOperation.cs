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

        public override void Handle(AvatarModel model)
        {
            Avatar avatar = avatarsPool.GetInactive(model.src);
            if (avatar != null) model.Complete(avatar);
            else if (avatarsPool.IsPoolMaxed(model.src)) model.Complete(avatarsPool.GetNextAvailable(model.src));
            else coroutine = StartCoroutine(Download(model));
        }

        private IEnumerator Download(AvatarModel model)
        {
            string src = model.src;
            Avatar avatar;
            AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(model.src);
            yield return locationsHandle;
            AsyncOperationHandle<GameObject> handle;
            if (locationsHandle.Result.Count > 0)
            {
                handle = Addressables.InstantiateAsync(model.src);
                yield return handle;
                avatar = handle.Result.GetComponent<Avatar>();
                Debug.Log($"[AddressableAvatarOperation] Loaded {model.src}");
                avatarsPool.AddAvatar(avatar, src);
                model.Complete(avatar);
            }
            else
            {
                Debug.Log($"[AddressableAvatarOperation] Failed to load {model.src}. Using {defaultKey} instead with addressable avatars.");
                model.src = defaultKey;
                Handle(model);
            }
            
        }

        public override void Stop()
        {
            StopCoroutine(coroutine);
        }
    }
}