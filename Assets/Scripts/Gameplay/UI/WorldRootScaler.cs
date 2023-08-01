using UnityEngine;

namespace Gameplay.UI
{
    public class WorldRootScaler : MonoBehaviour
    {
        private const float TargetAspectRatio = 9f / 16f;

        [SerializeField] private Transform _worldRoot;

        private void Start()
        {
            var currentAspectRatio = (float)Screen.width / Screen.height;

            if (!(Mathf.Abs(currentAspectRatio - TargetAspectRatio) > 0.01f))
            {
                return;
            }
            
            var scaleFactor = new Vector3(TargetAspectRatio / currentAspectRatio, 1f, TargetAspectRatio / currentAspectRatio);
            _worldRoot.localScale = Vector3.Scale(_worldRoot.localScale, scaleFactor);
        }
    }
}