using UnityEngine;


namespace com.outrealxr.avatars
{
    public class CharacterHandle : CustomYieldInstruction
    {
        public string Source;
        public virtual GameObject Character { get; set; }
        public virtual float PercentComplete { get; set; }

        public bool isReady = false;
        public override bool keepWaiting => !isReady;
    }
}