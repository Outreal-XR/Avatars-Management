using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace com.outrealxr.avatars
{
    public class AvatarsController : MonoBehaviour
    {
        public AvatarModel prefab;
        protected Dictionary<int, AvatarModel> models = new Dictionary<int, AvatarModel>();
        Queue<int> usersToServe = new Queue<int>();
        int currentlyServing = -1;

        public void AddModel(int userid, Transform transform, IPlayerAnimation sfsPlayerAnimation = null) {
            if (models.Keys.Contains(userid)) models[userid].gameObject.SetActive(true);
            else models.Add(userid, Instantiate(prefab, transform));
            models[userid].SetUserid(userid);
            models[userid].SetSFSPlayerAnimation(sfsPlayerAnimation);
        }

        public void UpdateModel(int userid, string src)
        {
            if (usersToServe.Contains(userid))
            {
                Debug.LogWarning($"[AvatarsController] Can't queue {userid} because it is already queued");
                return;
            }
            if (currentlyServing == userid)
            {
                Debug.LogWarning($"[AvatarsController] Can't queue {userid} because it is currently served");
                return;
            }
            models[userid].SetSource(src);

            usersToServe.Enqueue(userid);
            models[userid].SetQueued(true);
            TryServe();

        }

        public string GetAvatarSource(int userid)
        {
            return models[userid].GetSource();
        }

        private void TryServe() {
            if (currentlyServing > -1 || usersToServe.Count <= 0) return;
            
            currentlyServing = usersToServe.Dequeue();
            StartCoroutine(Serve());
        }

        IEnumerator Serve()
        {
            models[currentlyServing].HandleUpdate();
            yield return new WaitWhile(models[currentlyServing].IsLoading);
            models[currentlyServing].SetQueued(false);
            currentlyServing = -1;
            TryServe();
        }
    }
}