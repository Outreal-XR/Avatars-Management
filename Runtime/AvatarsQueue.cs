using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.outrealxr.avatars
{
    public class AvatarsQueue : MonoBehaviour
    {
        public Queue<AvatarModel> models = new Queue<AvatarModel>();

        public static AvatarsQueue instance;
        public static AvatarModel current;

        private void Awake()
        {
            instance = this;
        }

        public void Enqueue(AvatarModel model)
        {
            model.SetIsLoading(true);
            models.Enqueue(model);
            TryNext();
        }

        public void TryNext()
        {
            if (models.Count > 0)
            {
                if (current == null || !current.isLoading)
                {
                    current = models.Dequeue();
                    current.Apply();
                }
            }
            else
            {
                current = null;
            }
        }
    }
}