using UnityEngine;

namespace com.outrealxr.avatars
{
    public class AvatarsProvider : MonoBehaviour
    {

        public AvatarLoadingOperation[] avatarLoadingOperations;

        int currentID;
        public AvatarLoadingOperation currentOperation;
        public static AvatarsProvider instance;

        private void Awake()
        {
            instance = this;
        }

        public void LoadAvatar(AvatarModel model)
        {
            currentID = model.GetInstanceID();
            currentOperation = avatarLoadingOperations[model.type];
            currentOperation.Handle(model);
        }

        public bool IsLoading(AvatarModel model)
        {
            return model.GetInstanceID() == currentID && model.isLoading;
        }
    }
}