using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.outrealxr.avatars.ManyToMany
{
    [CreateAssetMenu(fileName = "New Avatar Data", menuName = "HoloFair/Create Avatar Data")]
    public class AvatarSelectModel : ScriptableObject
    {
        [field: SerializeField] public Sprite Image { get; private set; }
        [field: SerializeField] public AssetReference AvatarAsset { get; private set; }
    }
}