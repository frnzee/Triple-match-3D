using Zenject;
using UnityEngine.SceneManagement;

namespace Services
{
    public class SceneNavigation
    {
        private SceneNames _sceneNames;

        [Inject]
        public void Construct(SceneNames sceneNames)
        {
            _sceneNames = sceneNames;
        }

        public void LoadLevel(int levelNumber)
        {
            SceneManager.LoadScene("Level" + levelNumber);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(_sceneNames.MainMenu);
        }
    }
}