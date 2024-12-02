using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace MrSanmi.DungeonCrawler
{
    public class UIManager : MonoBehaviour
    {
        #region References

        public static UIManager instance;  // Singleton :P

        [SerializeField] protected GameReferee _gameReferee;

        [SerializeField] protected Dictionary<PlayersAvatar, playerIndex> playersDictionary;

        [SerializeField] protected GameObject[,] _healthIconsMatrix;
        [SerializeField] protected GameObject[] _healthIcons;
        [SerializeField] protected GameObject[] _healthIconsGroup;

        [SerializeField] protected GameObject[] _playersTextUI;

        #endregion

        #region UnityMethods

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        void Start()
        {
            playersDictionary = new Dictionary<PlayersAvatar, playerIndex>();
            PrepareUIHealthMatrix();
        }

        #endregion

        #region LocalMethods

        protected void PrepareUIHealthMatrix()
        {
            _healthIconsMatrix = new GameObject[4 , 5];

            for (short i = 0; i < _healthIconsGroup.Length; ++i)
            {
                _playersTextUI[i].gameObject.SetActive(false);
                for (short j = 0; j < _healthIcons.Length; ++j)
                {
                    _healthIconsMatrix[i, j] = _healthIconsGroup[i].transform.GetChild(j).gameObject;
                }
            }
        }

        protected void AssignHealthUI(PlayersAvatar playersAvatar, playerIndex playerIndex)
        {
            _playersTextUI[(short)playerIndex].gameObject.SetActive(true);

            for (short i = 0; i < playersAvatar.maxHealthPoints; ++i)
            {
                _healthIconsMatrix[(short)playerIndex, i].gameObject.SetActive(true);
            }
        }

        #endregion

        #region PublicMethods

        public void AddPlayerToDictionary(PlayersAvatar playersAvatar, playerIndex playerIndex)
        {
            playersDictionary.Add(playersAvatar, playerIndex);
            AssignHealthUI(playersAvatar, playerIndex);
        }

        // This didn't work properly, because it never reached the correct index when it came to the last or first index ;-;
        //public void UpdateHeartUIIcons(PlayersAvatar playersAvatar, short currentPlayerHP, string operation)
        //{
        //    if(playersDictionary.TryGetValue(playersAvatar, out playerIndex playerIndex))
        //    {
        //        for (short i = 0; i < playersAvatar.maxHealthPoints; ++i)
        //        {
        //            switch (operation)
        //            {
        //                case "Add":
        //                    if ((currentPlayerHP >= 1) && (currentPlayerHP < playersAvatar.maxHealthPoints))
        //                    {
        //                        healthIconsMatrix[(short)playerIndex, currentPlayerHP].gameObject.SetActive(true);
        //                        print(this.gameObject.name + " yessssssss, my hearts!" + (currentPlayerHP - 1).ToString());
        //                    }
        //                    else if(currentPlayerHP >= playersAvatar.maxHealthPoints)
        //                    {
        //                        healthIconsMatrix[(short)playerIndex, currentPlayerHP - 1].gameObject.SetActive(true);
        //                        print(this.gameObject.name + " yessssssss, my hearts!" + (currentPlayerHP - 1).ToString());
        //                    }
        //                    break;
        //                case "Substract":
        //                    if (currentPlayerHP <= 0)
        //                    {
        //                        healthIconsGroup[(short)playerIndex].gameObject.SetActive(false);
        //                    }
        //                    else if(currentPlayerHP >= 1)
        //                    {
        //                        healthIconsMatrix[(short)playerIndex, currentPlayerHP - 1].gameObject.SetActive(false);
        //                    }
        //                    break;
        //            }
        //        }
        //    }
        //}

        public void UpdateHeartUIIcons(PlayersAvatar playersAvatar, short currentPlayerHP)
        {
            if (playersDictionary.TryGetValue(playersAvatar, out playerIndex playerIndex))
            {
                for (short i = 0; i < playersAvatar.maxHealthPoints; ++i)
                {
                    if (i < currentPlayerHP)
                    {
                        _healthIconsMatrix[(short)playerIndex, i].gameObject.SetActive(true);
                    }
                    else
                    {
                        _healthIconsMatrix[(short)playerIndex, i].gameObject.SetActive(false);
                    }
                }
                if(currentPlayerHP <= 0)
                {
                    _playersTextUI[(short)playerIndex].gameObject.SetActive(false);
                }
            }

        }

        #endregion
    }

}