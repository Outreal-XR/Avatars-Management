using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace com.outrealxr.avatars
{
    [RequireComponent(typeof(AvatarController))]
    public class AvatarView : MonoBehaviour
    {
        AvatarController controller;

        public Avatar avatar { get; private set; }

        public TextMeshPro progressText;
        public GameObject loadingVisual, waitingVisual;
        public UnityEvent OnReveal, OnConceal;

        private void Awake()
        {
            controller = GetComponent<AvatarController>();
        }

        private void Start()
        {
            if(progressText == null) progressText = GetComponentInChildren<TextMeshPro>();
        }

        /// <summary>
        /// Must be called by input system whenever user hovers mouse on a collider of avatar
        /// </summary>
        public void RequestToReveal(string src)
        {
            if (!loadingVisual.activeSelf) controller.UpdateModel(src);
        }

        internal void Reveal()
        {
            avatar = GetComponentInChildren<Avatar>();
            if (avatar == null) return;
            var animator = avatar.GetComponent<Animator>();
            if (animator == null) return;
            animator.applyRootMotion = false;
            OnReveal.Invoke();
        }

        internal void Conceal()
        {
            avatar = null;
            OnConceal.Invoke();
        }
    }
}