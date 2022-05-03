using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.outrealxr.avatars
{
    public class ModelHandler : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public List<AvatarView> pool = new List<AvatarView>();
        }

        [SerializeField] private int maxAmountPerLabel = 5;
        
        //Ready Player Me
        [SerializeField] private Pool rpmPool = new Pool {pool = new List<AvatarView>()};
        [SerializeField] private List<GameObject> rpmToBeDestroyed = new List<GameObject>();

        //Addressable
        [SerializeField] private Dictionary<string, Pool> addressablesUsing = new Dictionary<string, Pool>();
        [SerializeField] private Dictionary<string, List<GameObject>> addressablesUnused = new Dictionary<string, List<GameObject>>();
        
        
        public CharacterHandle LoadAvatar(string url, Transform modelTransform) {
            CharacterHandle handle;

            if (url.StartsWith("https://") && url.EndsWith(".glb")) { //Case: ReadyPlayerMe
                handle = ReadyPlayerMeLoader.LoadAvatar(url, modelTransform); //TODO: This may crash a web page if too many avatars are loaded. Make sure it allows to cache max 10 and disposes them

                if (IsURLPoolMaxed (url)) {
                    var existingAvatar = rpmPool.pool[0].Avatar;
                    
                    rpmPool.pool[0].Conceal();
                    rpmPool.pool.RemoveAt(0);

                    rpmToBeDestroyed.Add(existingAvatar);
                }
                
            } else { //Default case: Addressable
                if (HasInactiveAvailable(url)) {
                    var existingAvatar = addressablesUnused[url][0];

                    addressablesUnused[url].RemoveAt(0);

                    existingAvatar.SetActive(true);
                    existingAvatar.transform.SetParent(modelTransform);
                    existingAvatar.transform.SetSiblingIndex(1);
                
                    existingAvatar.transform.localPosition = Vector3.zero;
                    existingAvatar.transform.localRotation = Quaternion.identity;
                    
                    handle = new CharacterHandle {Source = url, Character = existingAvatar, isReady = true, PercentComplete = 1f};

                } else if (!IsURLPoolMaxed(url)) 
                    handle = AddressableLoader.LoadAvatar(url, modelTransform);
                else {
                    var existingAvatar = addressablesUsing[url].pool[0].Avatar;

                    if (existingAvatar != null) {
                        existingAvatar.SetActive(true);
                        existingAvatar.transform.SetParent(modelTransform);
                        existingAvatar.transform.SetSiblingIndex(1);
                
                        existingAvatar.transform.localPosition = Vector3.zero;
                        existingAvatar.transform.localRotation = Quaternion.identity;
                
                        addressablesUsing[url].pool[0].Conceal();
                        addressablesUsing[url].pool.RemoveAt(0);
            
                        handle = new CharacterHandle {Source = url, Character = existingAvatar.gameObject, isReady = true, PercentComplete = 1f};
                    } else {
                        addressablesUsing[url].pool.RemoveAt(0);
                        handle = AddressableLoader.LoadAvatar(url, modelTransform);
                    }

                }
            }

            return handle;
        }

        private void LateUpdate() {
            foreach (var obj in rpmToBeDestroyed)
                Destroy(obj);
        
            rpmToBeDestroyed.Clear();
        }


        public void FreeUpAvatar(string url, GameObject character, AvatarView view) {
            character.SetActive(false);
            
            if (url.StartsWith("https://") && url.EndsWith(".glb")) {
                rpmPool.pool.Remove(view);
                rpmToBeDestroyed.Add(character);
            } else {
                addressablesUsing[url].pool.Remove(view);
                AddToUnused(url, character);
            }
        }

        private void AddToUnused(string url, GameObject avatar) {
            if (url.StartsWith("https://") && url.EndsWith(".glb")) return;

            if (!addressablesUnused.ContainsKey(url)) addressablesUnused.Add(url, new List<GameObject>());
            
            addressablesUnused[url].Add(avatar);
        }

        public void AddPool(string url) {
            if (url.StartsWith("https://") && url.EndsWith(".glb")) return;
            if (addressablesUsing.ContainsKey(url)) return;
            
            addressablesUsing.Add(url, new Pool());
        }

        public void AddToPool(string url, AvatarView view) {
            if (url.StartsWith("https://") && url.EndsWith(".glb")) {
                rpmPool.pool.Add(view);
                return;
            }
            
            if (!addressablesUsing.ContainsKey(url)) AddPool(url);
            
            addressablesUsing[url].pool.Add(view);
        }

        public bool CanSkipQueue(string url) => HasInactiveAvailable(url);
        public bool HasInactiveAvailable(string url) {
            if (url.StartsWith("https://") && url.EndsWith(".glb")) return false;

            return addressablesUnused.ContainsKey(url) && addressablesUnused[url].Count > 0;
        }

        public bool IsURLPoolMaxed(string url) {
            if (url.StartsWith("https://") && url.EndsWith(".glb")) return rpmPool.pool.Count >= maxAmountPerLabel;

            if (!addressablesUsing.ContainsKey(url)) 
                AddPool(url);

            return addressablesUsing[url].pool.Count >= maxAmountPerLabel;
        }
        
        private static ModelHandler _instance;

        public static ModelHandler instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindObjectOfType<ModelHandler> ();
                if (_instance != null) return _instance;
                
                var obj = new GameObject {
                    name = nameof(ModelHandler)
                };
                
                _instance = obj.AddComponent<ModelHandler> ();
                return _instance;
            }
        }
        
        private void Awake() {
            if (_instance == null) {
                _instance = this;
                if (transform.parent == null)
                    DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}
