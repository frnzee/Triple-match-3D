using UnityEngine;

namespace Gameplay.UI
{
    public class LevelDesignController : MonoBehaviour
    {
        [SerializeField] private Material[] _materials;
        [SerializeField] private Renderer[] _fieldEntities;
        
        private void Start()
        {
            var randomNumber = Random.Range(0, _materials.Length - 1);
            foreach (var unit in _fieldEntities)
            {
                unit.material = _materials[randomNumber];
            }
        }
    }
}