using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace com.outrealxr.avatars
{
    public class ReadyPlayerMeAvatarRequester : MonoBehaviour
    {
        
        // Animation avatar and controllers
        private const string AnimationTargetName = "ReadyPlayerMeAnimations/MaleAnimationTarget";
        private const string ControllerName = "ReadyPlayerMeAnimations/MaleFullbody";

        
        public void RequestAvatarGLB (string url, Action<GameObject> onSucceed, Action onFail) {
            StartCoroutine(SendRequest(url, onSucceed, onFail));
        }
        
        private IEnumerator SendRequest(string url, Action<GameObject> onSucceed, Action onFail) {
            var www = UnityWebRequest.Get(url);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success) {
                var results = www.downloadHandler.data;

                var avatar = new GameObject();

                //Armature
                var armature = new GameObject {
                    name = "Armature",
                    transform = {
                        parent = avatar.transform
                    }
                };
                var hips = avatar.transform.Find("Hips");
                hips.parent = armature.transform;
                
                //Adding animator
                var animationAvatar = Addressables.LoadAssetAsync<Avatar>(AnimationTargetName);
                yield return animationAvatar;

                var animatorController = Addressables.LoadAssetAsync<RuntimeAnimatorController>(ControllerName);
                yield return animatorController;
                
                var animator = avatar.AddComponent<Animator>();
                
                animator.runtimeAnimatorController = animatorController.Result;
                animator.avatar = animationAvatar.Result;
                animator.applyRootMotion = true;
                
                onSucceed.Invoke(avatar);
            } else {
                onFail.Invoke();
            }

        }
    }
    
}