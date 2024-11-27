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

        public static UIManager instance;

        [SerializeField] protected GameReferee _gameReferee;

        protected HashSet<PlayersAvatar> playersSet;

        [SerializeField] protected GameObject[] healthIcons;
        [SerializeField] protected GameObject[] healthIconsGroup;

        #endregion

        #region UnityMethods

        private void Awake()
        {
            //instance == null ? instance = this : Destroy(this);

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
            playersSet = new HashSet<PlayersAvatar>();
        }

        #endregion

        #region PublicMethods

        public void AddPlayerToSet(PlayersAvatar p_playersAvatar)
        {
            playersSet.Add(p_playersAvatar);
            Debug.Log(playersSet.Count);
        }

        #endregion
    }

}