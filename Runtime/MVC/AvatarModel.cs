using System;
using System.Collections;
using UnityEngine;

namespace com.outrealxr.avatars
{
    public class AvatarModel : MonoBehaviour
    {
        AvatarView view;
        private string src;
        bool isLocal;
        Coroutine coroutine;
        IPlayerAnimation playerAnimation;

        private CharacterHandle _handle;

        private void Awake()
        {
            if ( _instance == null ) {
                _instance = this;
                if (transform.parent == null)
                    DontDestroyOnLoad ( gameObject );
            }
            else
            {
                Destroy ( gameObject );
            }
            
            view = GetComponent<AvatarView>();
        }

        private void FixedUpdate()
        {
            if (IsLoading()) view.progressText.text = $"{_handle.PercentComplete:P2}.";
        }

        public void SetUserid(int userid)
        {
            view.SetUserid(userid);
        }

        public void SetSource(string src)
        {
            this.src = src;
            view.SetLabel(src);
        }

        public string GetSource()
        {
            return src;
        }

        internal void SetSFSPlayerAnimation(IPlayerAnimation sfsPlayerAnimation)
        {
            this.playerAnimation = sfsPlayerAnimation;
        }

        public void SetQueued(bool queued)
        {
            view.waitingVisual.SetActive(queued);
            view.progressText.gameObject.SetActive(!queued);
            view.loadingVisual.SetActive(queued);
        }

        public void HandleUpdate()
        {
            if(gameObject.activeInHierarchy)
                coroutine = StartCoroutine(Download());
        }

        private IEnumerator Download() {
            
            if (view.Avatar != null)
                ModelHandler.instance.FreeUpAvatar(_handle.Source, view.Avatar, view);
            
            _handle = ModelHandler.instance.LoadAvatar(src, transform);
            yield return _handle;

            ModelHandler.instance.AddToPool(src, view);


            _handle.Character.transform.SetAsFirstSibling();
            view.Reveal(_handle.Character);
            coroutine = null;
            playerAnimation?.ReadUserVariable();
        }

        private void OnDisable()
        {
            if(coroutine != null)
                StopCoroutine(coroutine);
        }

        public bool IsLoading() => _handle is {keepWaiting: true};

        
        private static AvatarModel _instance;

        public static AvatarModel instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindObjectOfType<AvatarModel> ();
                if (_instance != null) return _instance;
                
                var obj = new GameObject {
                    name = nameof(AvatarModel)
                };
                
                _instance = obj.AddComponent<AvatarModel> ();
                return _instance;
            }
        }
        
    }
}