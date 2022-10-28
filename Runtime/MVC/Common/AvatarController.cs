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
            UpdateModel(model.src, true);
        }

        public void UpdateModel(string src, bool forced)
        {
            model.SetSource(src);

            if (model.type == 0)
                forced = true;
            else if (model.type == 1) 
                forced = !AvatarsQueue.instance.IsPoolMaxed(model.type, model.src);
            
            if (model.HasAvatar || forced || model.isLocal)
                AvatarsQueue.instance.Enqueue(model);
        }

    }
}