using Zenject;
using UnityEngine.SceneManagement;

namespace Services
{
    public class SceneNavigation
    {
        private const string SceneNameTemplate = "Level ";
        private SceneNames _sceneNames;

        [Inject]
        public void Construct(SceneNames sceneNames)
        {
            _sceneNames = sceneNames;
        }

        public static void LoadLevel(int levelNumber)
        {
            SceneManager.LoadScene(SceneNameTemplate + levelNumber);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(_sceneNames.MainMenu);
        }
    }
}