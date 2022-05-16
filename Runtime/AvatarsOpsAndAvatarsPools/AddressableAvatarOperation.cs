using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace com.outrealxr.avatars
{
    [RequireComponent(typeof(AddressableAvatarPool))]
    public class AddressableAvatarOperation : AvatarLoadingOperation
    {

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
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(model.src);
            yield return handle;
            avatarsPool.AddAvatar(handle.Result.GetComponent<Avatar>(), model.src);
            model.Complete(handle.Result.GetComponent<Avatar>());
            Debug.Log($"[AddressableAvatarOperation] Loaded {model.src}");
        }

        public override void Stop()
        {
            StopCoroutine(coroutine);
        }
    }
}