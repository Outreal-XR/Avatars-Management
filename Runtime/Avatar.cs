using UnityEngine;

namespace com.outrealxr.avatars
{
    public class Avatar : MonoBehaviour
    {
        public AvatarModel owner;

        public void SetOwner(AvatarModel owner)
        {
            if (this.owner != null) owner.AvatarRemoved();
            this.owner = owner;
            transform.parent = owner.transform;
            transform.localPosition = Vector3.zero;
            transform.rotation = Quaternion.identity;
            this.owner.AvatarAssigned();
        }
    }
}