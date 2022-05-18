using ReadyPlayerMe;
using UnityEngine;

namespace com.outrealxr.avatars
{
    public class RPMRequestHandle : CustomYieldInstruction
    {
        public static RPMRequestHandle Request(string url) {
            var readyPlayerMeHandler = new RPMRequestHandle {
                Source = url
            };

            var avatarLoader = new AvatarLoader();
            avatarLoader.LoadAvatar(url, avatar => { }, (avatar, metaData) => {
                readyPlayerMeHandler.Character = avatar;
                readyPlayerMeHandler.Character.transform.localPosition = Vector3.zero;
                readyPlayerMeHandler.Character.transform.localRotation = Quaternion.identity;
                readyPlayerMeHandler.IsReady = true;
            });

            return readyPlayerMeHandler;
        }
        private RPMRequestHandle() { }
        
        public string Source;
        public GameObject Character;
        public bool IsReady = false;
        public override bool keepWaiting => IsReady;
    }
}