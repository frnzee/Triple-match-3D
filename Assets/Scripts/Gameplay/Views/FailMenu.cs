using UnityEngine;
using Zenject;

namespace Gameplay.Views
{
    public class FailMenu : MonoBehaviour
    {
        [Inject]
        public void Construct(Transform parentTransform)
        {
            transform.SetParent(parentTransform, false);
        }

        public class Factory : PlaceholderFactory<Transform, FailMenu>
        {
        }
    }
}