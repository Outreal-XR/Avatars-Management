using UnityEngine;

namespace com.outrealxr.avatars
{
    [RequireComponent(typeof(AvatarView))]
    public class AvatarModel : MonoBehaviour
    {
        AvatarView view;
        Avatar current;

        public int type;
        public string src;
        public bool isLoading;
        public bool isLocal;
        IPlayerAnimation playerAnimation;

        public static AvatarModel instance;

        private void Awake()
        {
            view = GetComponent<AvatarView>();
            SetIsLocal(isLocal);
        }

        private void Start()
        {
            view.RequestToReveal(src);
        }

        public void SetIsLocal(bool val)
        {
            isLocal = val;
            if (isLocal) instance = this;
        }

        public void UpdateLoading(float amount)
        {
            view.progressText.text = $"{amount:P2}";
        }

        public void SetSource(string src)
        {
            this.src = src;
        }

        internal void SetSFSPlayerAnimation(IPlayerAnimation playerAnimation)
        {
            this.playerAnimation = playerAnimation;
        }

        public void SetIsLoading(bool val)
        {
            if (view)
            {
                if(view.waitingVisual) view.waitingVisual.SetActive(val);
                else Debug.Log($"[AvatarModel] View.waitingVisual on {gameObject} is missing?");
                if (view.waitingVisual) view.loadingVisual.SetActive(val);
                else Debug.Log($"[AvatarModel] View.loadingVisual on {gameObject} is missing?");
            }
            else
            {
                Debug.Log($"[AvatarModel] View on {gameObject} is missing?");
            }
            isLoading = val;
        }

        public void Apply(int type, string url)
        {
            if (gameObject.activeInHierarchy) AvatarsProvider.instance.LoadAvatar(this, type, url);
            else Complete(current);
        }
        
        
        public void FreeUpAvatar() {
            if (!current) return;

            switch (current.type) {
                case 0:
                    current.SetOwner(null);
                    break;
                case 1:
                    Destroy(current.gameObject);
                    break;
            }
            
        }


        public void Complete(Avatar avatar) {
            FreeUpAvatar();
            
            current = avatar;
            if (current) {
                current.SetOwner(this);
                playerAnimation?.ReadUserVariable();
            }
            SetIsLoading(false);
            AvatarsQueue.instance.TryNext();
        }

        public void AvatarAssigned()
        {
            view.Reveal();
        }

        public void AvatarRemoved()
        {
            view.Conceal();
        }

        private void OnDisable()
        {
            if (AvatarsProvider.instance.IsLoading(this)) AvatarsProvider.instance.currentOperation.Stop();
        }
    }
}