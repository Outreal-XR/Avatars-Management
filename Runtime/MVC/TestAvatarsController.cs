using UnityEngine;
using Random = System.Random;

namespace com.outrealxr.avatars
{
    public class TestAvatarsController : AvatarsController
    {

        [System.Serializable]
        public class User
        {
            public string label;
            public Transform transform;
            public bool Skip;
        }

        public User[] users;
        public float waitBeforeUpdate = 2;

        public string[] avatarNames;

        private void Start()
        {
            foreach (var user in users)
            {
                if (!user.Skip)
                {
                    AddModel(user.transform.GetInstanceID(), user.transform);
                    UpdateModel(user.transform.GetInstanceID(), user.label);
                }
            }
            InvokeRepeating(nameof(SequentiallyUpdateFirstUser), waitBeforeUpdate, waitBeforeUpdate);
        }

        int currentIndex;

        void SequentiallyUpdateFirstUser()
        {
            var user = users[0];
            UpdateModel(user.transform.GetInstanceID(), avatarNames[currentIndex]);
            
            currentIndex++;
            currentIndex %= avatarNames.Length;

        }
    }
}