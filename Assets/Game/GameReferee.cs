using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrSanmi.Game
{
    #region Enum

    public enum GameStates
    {
        GAME,
        PAUSE,
        GAME_OVER, //GAME_OVER
        VICTORY
        //DRAW
    }

    #endregion

    public class GameReferee : MonoBehaviour
    {
        #region RuntimeVariables

        protected GameStates _gameState;

        #endregion

        #region UnityMethods
        void Start()
        {
            InitializeGameReferee();
        }
        private void Update()
        {
            print(_gameState.ToString());
        }

        #endregion

        #region RuntimeMethods

        protected virtual void InitializeGameReferee()
        {
            _gameState = GameStates.GAME;
        }

        public void SetPauseGameState()
        {
            if(_gameState == GameStates.GAME)
            {
                _gameState = GameStates.PAUSE;
                Time.timeScale = 0.0f;
            }
            else if(_gameState == GameStates.PAUSE)
            {
                _gameState = GameStates.GAME;
                Time.timeScale = 1.0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }

        #endregion
    }
}

