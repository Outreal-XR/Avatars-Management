using UnityEngine;
using UnityEngine.AddressableAssets;


namespace com.outrealxr.avatars
{
    public static class AddressableLoader
    {
        public static CharacterHandle LoadAvatar(string url, Transform modelTransform) {
            var readyPlayerMeHandler = new AddressableCharacterHandle {
                Source = url
            };

            var addressableHandle = Addressables.InstantiateAsync(url, modelTransform);
            readyPlayerMeHandler.Addressable = addressableHandle;

            return readyPlayerMeHandler;
        }
    }
}