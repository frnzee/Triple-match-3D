using UnityEngine;
using Zenject;

namespace Gameplay.Boosters
{
    public class BoosterCollection : MonoBehaviour
    {
        private Fan.Factory _fanFactory;
        
        [Inject]
        public void Construct(Fan.Factory fanFactory)
        {
            _fanFactory = fanFactory;
        }
        
        public void UseFan()
        {
            _fanFactory.Create();
        }
    }
}
