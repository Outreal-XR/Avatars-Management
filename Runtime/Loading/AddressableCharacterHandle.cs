using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace com.outrealxr.avatars
{
    public class AddressableCharacterHandle : CharacterHandle
    {
        public AsyncOperationHandle<GameObject> Addressable;

        public override float PercentComplete => Addressable.PercentComplete;
        public override GameObject Character => Addressable.Result;
        public override bool keepWaiting => !(Addressable.Status is AsyncOperationStatus.Succeeded);

    }
}