using UnityEngine.SceneManagement;

namespace Services
{
    public class SceneNavigation
    {
        private const string SceneNameTemplate = "Level ";
        private const string MainMenu = "MainMenu";

        public static void LoadLevel(int levelNumber)
        {
            SceneManager.LoadScene(SceneNameTemplate + levelNumber);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(MainMenu);
        }
    }
}