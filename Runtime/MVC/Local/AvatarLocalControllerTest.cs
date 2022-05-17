using UnityEngine;

namespace com.outrealxr.avatars
{
    public class AvatarLocalControllerTest : AvatarLocalController
    {
        public override void UpdateLocalModel(string src)
        {
            AvatarModel.instance.gameObject.GetComponent<AvatarView>().RequestToReveal(src);
        }
    }
}