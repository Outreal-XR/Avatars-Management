using System.Collections.Generic;
using UnityEngine;

namespace com.outrealxr.avatars.revised
{
    public class AvatarsModel : MonoBehaviour
    {
        public Dictionary<int, AvatarModel> avatars = new Dictionary<int, AvatarModel>();

        public void UpdateAvatarModel(int id, AvatarView view, string src)
        {
            if (!avatars.ContainsKey(id))
            {
                avatars.Add(id, new AvatarModel(src, view));
                view.SetModel(avatars[id]);
            }
            avatars[id].SetSource(src);
        }
    }
}