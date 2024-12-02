using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MrSanmi.DungeonCrawler
{
    public class SceneChanger : MonoBehaviour
    {
        public static SceneChanger instance; // Singleton :P

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        //Scene IDs:
        // - 0: Main Title Screen.
        // - 1: Gameplay Scene.
        // - 2: Victory Scene.
        // - 3: Game Over Scene.
        public void ChangeSceneTo(short sceneID)
        {
            SceneManager.LoadScene(sceneID);
        }

        // It makes you quit the game :P
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}