using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace com.outrealxr.avatars
{
    [RequireComponent(typeof(AvatarController))]
    public class AvatarView : MonoBehaviour
    {
        AvatarController controller;

        public GameObject Avatar { get; private set; }

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
        public void RequestToReveal(string newLabel)
        {
            if (Avatar == null && !loadingVisual.activeSelf) controller.UpdateModel(newLabel);
        }

        internal void Reveal()
        {
            Avatar = GetComponentInChildren<Avatar>().gameObject;
            OnReveal.Invoke();
            
            if (Avatar == null) return;

            var animator = Avatar.GetComponent<Animator>();
 
            if (animator == null) return;
            
            animator.applyRootMotion = false;
            var animatorParameters = Avatar.GetComponent<AnimatorParameters>();
            if (animatorParameters == null) Avatar.AddComponent<AnimatorParameters>();
        }

        internal void Conceal()
        {
            Avatar = null;
            OnConceal.Invoke();
        }
    }
}