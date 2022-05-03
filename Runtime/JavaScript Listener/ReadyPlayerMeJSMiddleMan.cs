using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

namespace com.outrealxr.avatars
{
    public class ReadyPlayerMeJSMiddleMan : MonoBehaviour
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void openRPMIframe();
#endif

        public void OpenIFrame() {
#if UNITY_WEBGL && !UNITY_EDITOR
            openRPMIframe();
#endif
        }

        public UnityEvent<string> OnReadyPlayerMeURLReceive = new UnityEvent<string>();

        public void ReceiveMessage(string url) {    
            OnReadyPlayerMeURLReceive.Invoke(url);
            Debug.Log($"Received URL from JavaScript: {url}");
        }
        
        private static ReadyPlayerMeJSMiddleMan _instance;

        public static ReadyPlayerMeJSMiddleMan instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindObjectOfType<ReadyPlayerMeJSMiddleMan> ();
                if (_instance != null) return _instance;
                
                var obj = new GameObject {
                    name = nameof(ReadyPlayerMeJSMiddleMan)
                };
                
                _instance = obj.AddComponent<ReadyPlayerMeJSMiddleMan> ();
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