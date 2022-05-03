using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace com.outrealxr.avatars
{
    public class AvatarView : MonoBehaviour
    {
        AvatarsController controller;

        int userid;
        string label;

        public GameObject Avatar { get; private set; }

        public TextMeshPro progressText;
        public GameObject loadingVisual, waitingVisual;
        public UnityEvent OnReveal, OnConceal;

        private void Start()
        {
            controller = FindObjectOfType<AvatarsController>();
            if(progressText == null) progressText = GetComponentInChildren<TextMeshPro>();
        }

        public void SetUserid(int userid)
        {
            this.userid = userid;
        }

        public void SetLabel(string label)
        {
            this.label = label;
        }

        /// <summary>
        /// Must be called by input system whenever user hovers mouse on a collider of avatar
        /// </summary>
        public void RequestToReveal()
        {
            if (Avatar == null && !loadingVisual.activeSelf) controller.UpdateModel(userid, label);
        }

        internal void Reveal(GameObject avatar)
        {
            Avatar = avatar;
            OnReveal.Invoke();
            
            if (avatar == null) return;

            var animator = avatar.GetComponent<Animator>();
 
            if (animator == null) return;
            
            animator.applyRootMotion = false;
            var animatorParameters = avatar.GetComponent<AnimatorParameters>();
            if (animatorParameters == null) avatar.AddComponent<AnimatorParameters>();
        }

        internal void Conceal()
        {
            Avatar = null;
            OnConceal.Invoke();
        }
    }
}