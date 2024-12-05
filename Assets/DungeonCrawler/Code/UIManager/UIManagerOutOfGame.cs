using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrSanmi.DungeonCrawler
{
    public class UIManagerOutOfGame : MonoBehaviour
    {
        public static UIManagerOutOfGame instance;

        [SerializeField] protected GameObject _creditsPanel;
        [SerializeField] protected GameObject _mainMenuPanel;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(instance);
            }
        }

        public void ActivateCreditsPanel()
        {
            _mainMenuPanel.SetActive(false);
            _creditsPanel.SetActive(true);
        }

        public void ActivateMainMenuPanel()
        {
            _mainMenuPanel.SetActive(true);
            _creditsPanel.SetActive(false);
        }
    }

}