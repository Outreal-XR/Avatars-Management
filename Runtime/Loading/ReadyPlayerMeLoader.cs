using UnityEngine;

namespace com.outrealxr.avatars
{
    public static class ReadyPlayerMeLoader
    {
        public static CharacterHandle LoadAvatar(string url, Transform modelTransform) {
            var readyPlayerMeHandler = new CharacterHandle {
                Source = url
            };

            return readyPlayerMeHandler;
        }
    }
}