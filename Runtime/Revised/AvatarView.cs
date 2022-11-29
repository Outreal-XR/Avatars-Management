using UnityEngine;

namespace com.outrealxr.avatars.revised
{
    public class AvatarView : MonoBehaviour
    {
        State state;
        public GameObject loading, queue, placeholder;
        public UnityEngine.Events.UnityEvent OnReveal, OnConceal;
        AvatarModel model;

        void OnEnable()
        {
            SetState(State.None);
            Reveal();
        }

        public void SetModel(AvatarModel model)
        {
            this.model = model;
        }

        public void SetState(State state)
        {
            loading.SetActive(state == State.Loading);
            queue.SetActive(state == State.Queued);
            placeholder.SetActive(state == State.None);
            if (state == State.None) OnConceal.Invoke();
            else if (state == State.Set) OnReveal.Invoke();
            this.state = state;
        }

        /// <summary>
        /// Intended to be triggered by user input
        /// </summary>
        public void Reveal()
        {
            if(model != null && state != State.Queued) AvatarsQueue.instance.Enqueue(model);
        }
    }
}