using UnityEngine;

namespace com.outrealxr.avatars
{
    public class AvatarLocalView : MonoBehaviour
    {
        public CanvasGroup canvasGroup;

        public static AvatarLocalView instance;

        private void Awake()
        {
            instance = this;
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