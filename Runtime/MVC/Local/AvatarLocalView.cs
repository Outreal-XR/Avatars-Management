using UnityEngine;

namespace com.outrealxr.avatars
{
    public class AvatarLocalView : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public GameObject view;

        public static AvatarLocalView instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            if(PlayerPrefs.HasKey(AvatarLocalController.PreKey))
                AvatarLocalController.instance.UpdateLocalModel(PlayerPrefs.GetString(AvatarLocalController.PreKey));
            view.SetActive(!PlayerPrefs.HasKey(AvatarLocalController.PreKey) && AvatarLocalController.promptUserOnStart);
        }

        private void Update()
        {
            canvasGroup.interactable = !AvatarModel.instance.isLoading;
        }

        public void Select(string src)
        {
            AvatarLocalController.instance.UpdateLocalModel(src);
            PlayerPrefs.SetString(AvatarLocalController.PreKey, src);
        }
    }
}