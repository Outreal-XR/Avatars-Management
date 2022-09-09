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
            view.SetActive(!AvatarLocalController.instance.IsAvatarSet());
        }

        private void Update()
        {
            canvasGroup.interactable = !AvatarModel.instance.isLoading;
        }

        public void Select(string src)
        {
            AvatarLocalController.instance.UpdateLocalModel(src);
        }
    }
}