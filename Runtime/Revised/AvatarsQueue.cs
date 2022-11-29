using System.Collections.Generic;
using UnityEngine;

namespace com.outrealxr.avatars.revised
{
    public class AvatarsQueue : MonoBehaviour
    {

        public RPMAvatarOperation gltfAvatarOperation;
        public AddressableAvatarOperation addressableAvatarOperation;
        public int maxQueue = 25;
        AvatarLoadingOperation operation;

        AvatarModel current;
#if UNITY_EDITOR
        AvatarModel[] queueArray;
#endif
        Queue<AvatarModel> queue = new();

        public static AvatarsQueue instance;

        void Awake()
        {
            instance = this;
        }

        void Update()
        {
            if (queue.Count == 0) return;
            if (operation != null && operation.running) return;
            current = queue.Dequeue();
            current.Dequeued();
            queueArray = queue.ToArray();
            operation = addressableAvatarOperation;
            if (string.IsNullOrWhiteSpace(current.src)) Debug.LogError("[AvatarsQueue] AvatarModel.src can't be empty");
            if (current.src.EndsWith("glb") || current.src.EndsWith("gltf")) operation = gltfAvatarOperation;
            if (operation == null)
            {
                Debug.LogWarning($"[AvatarsQueue] No operation available for {current}. Skipped");
                return;
            }
            if (current.IsVisible()) operation.Handle(current);
            else Debug.LogWarning($"[AvatarsQueue] {current} is not visible anymore. Skipped");
        }

        public void Enqueue(AvatarModel model)
        {
            queue.Enqueue(model);
            model.Queued();
            if (queue.Count >= maxQueue) queue.Dequeue().SetAvatar(null);
        }

#if UNITY_EDITOR
        void OnGUI()
        {
            if (queueArray != null)
            {
                // Make a background box
                GUI.Box(new Rect(32, 32, 512, 64 + 32 * queueArray.Length), "Queue");
                GUI.Label(new Rect(32, 64, 512 - 32, 32), text: "Current: " + current);
                for (int i = 0; i < queueArray.Length; i++)
                    GUI.Label(new Rect(32, 96 + 32 * i, 512 - 32, 128 - 32), queueArray[i].ToString());
            }

        }
#endif
    }
}