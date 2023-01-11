using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace com.outrealxr.avatars.ManyToMany
{
    public class AvatarSelectFactory : MonoBehaviour
    {
        [SerializeField] private string _url;
        [SerializeField] private AvatarSelectViewPool _pool;

        public void FetchCatalogue() {
            _pool.ResetViews();
            StartCoroutine(RequestCatalogue());
        }

        private IEnumerator RequestCatalogue() {
            var www = UnityWebRequest.Get(_url);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www;

            if (www.result != UnityWebRequest.Result.Success) {
                //TODO Throw warning here
                yield break;
            }

            var array = JArray.Parse(www.downloadHandler.text);

            foreach (var jToken in array) {
                var catalogue = jToken.Value<string>();
                yield return Addressables.LoadContentCatalogAsync(catalogue, true);
            }
            
            var loadResourceLocationsHandle = Addressables.LoadResourceLocationsAsync("AvatarSelectData", typeof(AvatarSelectModel));
            yield return loadResourceLocationsHandle;
            
            if (loadResourceLocationsHandle.Status != AsyncOperationStatus.Succeeded) {
                //TODO Throw warning here
                yield break;
            }
            
            foreach (var location in loadResourceLocationsHandle.Result) {
                var loadAssetHandle = Addressables.LoadAssetAsync<AvatarSelectModel>(location);
                yield return loadAssetHandle;
                var model = loadAssetHandle.Result;
                
                _pool.UpdateView(model.Image, () => {
                    AvatarLocalController.instance.UpdateLocalModel(model.AvatarAsset.RuntimeKey.ToString());
                });
            }
        }

        public void SetUrl(string url) => _url = url;
    }
}