using Gameplay.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services
{
    public class SceneNavigation
    {
        private const string SceneNameTemplate = "Level";
        private const string MainMenu = "MainMenu";

        public static int CurrentGameLevel { get; private set; }
        
        public static void LoadLevel(int levelNumber)
        {
            CurrentGameLevel = levelNumber;
            SceneManager.LoadScene(SceneNameTemplate);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(MainMenu);
        }
    }
}