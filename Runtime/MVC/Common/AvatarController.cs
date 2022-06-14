using UnityEngine;

namespace com.outrealxr.avatars
{
    public class AvatarController : MonoBehaviour
    {
        AvatarModel model;

        private void Awake()
        {
            model = GetComponent<AvatarModel>();
        }

        public void RequestToRevealItself()
        {
            UpdateModel(model.src);
        }

        public void UpdateModel(string src)
        {
            model.SetSource(src);
            AvatarsQueue.instance.Enqueue(model);
        }

    }
}