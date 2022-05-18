using UnityEngine;

namespace com.outrealxr.avatars
{
    public abstract class AvatarLocalController : MonoBehaviour
    {
        public static string PreKey = "LastSelectedSrc";
        public static AvatarLocalController instance;
        public static bool promptUserOnStart = true;

        private void Awake()
        {
            instance = this;
        }

        public void SetPromptUserOnStart(bool val)
        {
            promptUserOnStart = val;
        }

        public abstract void UpdateLocalModel(string src);

        public virtual void ToggleSelection(bool val)
        {
            AvatarLocalView.instance.view.SetActive(val);
        }
    }
}