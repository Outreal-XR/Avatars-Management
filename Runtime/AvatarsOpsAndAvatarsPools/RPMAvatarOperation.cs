using System.Collections;
using GLTFast;
using UnityEngine;

namespace com.outrealxr.avatars
{
    public class RPMAvatarOperation : AvatarLoadingOperation
    {
        [SerializeField] private string defaultKey = "yBot";
        private Coroutine _coroutine;

        [SerializeField] private RuntimeAnimatorController runtimeAnimatorController;
        [SerializeField] private AddressableAvatarOperation addressableAvatarOperation;

        [SerializeField] private UnityEngine.Avatar animationAvatar;
        
        private void Awake()
        {
            avatarsPool = GetComponent<RPMAvatarPool>();
        }

        public override void Handle(AvatarModel model, string src) {
            _coroutine = StartCoroutine(LoadAvatar(model, src));
        }

        private IEnumerator LoadAvatar(AvatarModel model, string src) {
            var gltfHolder = new GameObject(RPMAvatarPool.GltfHolderName);
            var gltfAsset = gltfHolder.AddComponent<GltfAsset>();
            
            var handle = gltfAsset.Load(src);
            yield return handle;

            var timeWaited = 0f;
            while (gltfHolder.transform.childCount == 0) {
                yield return new WaitForSeconds(0.2f);
                timeWaited += 0.2f;

                //If we dont get the model in time, just dispose and delete it.
                if (timeWaited > 5f) {
                    gltfAsset.Dispose();
                    Destroy(gltfHolder);
                    OnLoadFailed(model);
                    yield break;
                }
            }

            gltfHolder.transform.SetParent(model.transform);
            gltfHolder.transform.localPosition = Vector3.zero;
            
            var obj = gltfHolder.transform.GetChild(0).GetChild(0).gameObject;
            obj.name = "Avatar";
            
            //Reorganize the object hierarchy to fit the RPM avatar 
            var armature = new GameObject("Armature");
            armature.transform.SetParent(obj.transform);
            var hips = obj.transform.Find("Hips");
            hips.SetParent(armature.transform);
            
            //Add the animator and assign the controller and avatar
            var animator = obj.AddComponent<Animator>();
            animator.runtimeAnimatorController = runtimeAnimatorController;
            animator.avatar = animationAvatar;
            
            var avatar = obj.AddComponent<Avatar>();
            avatar.SetOwner(model);
            avatar.type = AvatarsProvider.instance.avatarLoadingOperations.IndexOf(this);

            obj.AddComponent<AnimatorParameters>();

            Debug.Log($"[RPMAvatarOperation] Loaded {model.src}");
            avatarsPool.AddAvatar(avatar, src);
            model.Complete(avatar);
        }

        private void OnLoadFailed(AvatarModel model) {
            Debug.Log($"[RPMAvatarOperation] Failed to load {model.src}. Using {defaultKey} instead with addressable avatars.");
            addressableAvatarOperation.Handle(model, defaultKey);
        }

        public override void Stop() {
            StopCoroutine(_coroutine);
        }
    }
}