using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace com.outrealxr.avatars.revised
{
    public class AddressableAvatarOperation : AvatarLoadingOperation
    {
        public override void Handle(AvatarModel model)
        {
            running = true;
            StartCoroutine(Download(model));
        }

        private IEnumerator Download(AvatarModel model) {
            AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(model.src);
            yield return locationsHandle;
            AsyncOperationHandle<GameObject> handle;
            if (locationsHandle.Result.Count > 0)
            {
                handle = Addressables.InstantiateAsync(model.src);
                yield return handle;
                Debug.Log($"[AddressableAvatarOperation] Loaded {model.src}");
                model.SetAvatar(handle.Result);
            }
            else
            {
                Debug.Log($"[AddressableAvatarOperation] Failed to load {model.src}");
                model.SetAvatar(null);
            }
            running = false;
        }
    }
}