using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.outrealxr.avatars
{
    public class AvatarsQueue : MonoBehaviour
    {

        public class AvatarModelQueue
        {
            public int AvatarType;
            public string url;
            public AvatarModel model;
        }
        
        public Queue<AvatarModelQueue> queue = new();

        public static AvatarsQueue instance;
        public static AvatarModelQueue current;

        [SerializeField] private AvatarsProvider provider;
        
        private void Awake()
        {
            instance = this;

            if (!provider) provider = GetComponent<AvatarsProvider>();
        }
        
        public void Enqueue(AvatarModel model)
        {
            model.SetIsLoading(true);
            
            queue.Enqueue(new AvatarModelQueue{
                AvatarType = model.type,
                url = model.src,
                model = model
            });
            
            if (queue.Count == 1)
                TryNext();
        }

        public void TryNext()
        {
            if (queue.Count == 0) {
                current = null;
                return;
            }

            if (current == null || !current.model.isLoading) {
                current = queue.Dequeue();
                current.model.Apply(current.AvatarType, current.url);
            }
        }
    }
}