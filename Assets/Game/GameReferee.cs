using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace MrSanmi.DungeonCrawler
{
    #region Enums

    public enum GameStates
    {
        GAME, //= 0
        PAUSE, //= 1
        GAME_OVER, //= 2
        VICTORY
    }

    #endregion

    public class GameReferee : MonoBehaviour
    {
        #region References

        public GameObject panelPause;

        [SerializeField] public CinemachineTargetGroup targetGroup;

        [SerializeField] protected InteractiveStuff[] interactiveStuff;

        [SerializeField] protected GameObject orbe;
        [SerializeField] protected Transform[] orbePositions;

        [SerializeField] protected GameObject[] finalPathLocks;
        [SerializeField] protected GameObject[] middlePathLocks;
        [SerializeField] protected Portal[] portalsOfTheGame;

        #endregion

        #region RuntimeVariables

        [SerializeField] protected GameStates _gameState;
        [SerializeField] public bool orbeHasAlreadyBeenPlaced;

        #endregion

        #region UnityMethods
        void Start() //Runtime
        {
            InitializeGameState();
            orbeHasAlreadyBeenPlaced = false;
            orbe.gameObject.transform.position = orbePositions[0].gameObject.transform.position;
            orbe.gameObject.SetActive(false);
        }

        void FixedUpdate() //Update(): Max 200 FPS / FixedUpdate(): 50 FPS
        {
            switch (_gameState)
            {
                case GameStates.GAME:
                    ExecutingGameState();
                    break;
                case GameStates.GAME_OVER:
                    //ExecutingGameOverState();
                    break;
                case GameStates.VICTORY:
                    //ExecutingVictoryState();
                    break;
                case GameStates.PAUSE:
                    ExecutingPauseState();
                    break;
            }
        }

        #endregion

        #region PublicMethods

        public void DeactivatePathLocks(string typeOfLock)
        {
            switch (typeOfLock)
            {
                case "MiddlePathLocks":
                    foreach (GameObject pathLock in middlePathLocks)
                    {
                        pathLock.gameObject.SetActive(false);
                    }
                    break;
                case "FinalPathLocks":
                    foreach(GameObject pathLock in finalPathLocks)
                    {
                        pathLock.gameObject.SetActive(false);
                    }
                    break;
            }
        }

        public void ActivateOrbeAtTheBeginningOfTheGame()
        {
            if (!orbeHasAlreadyBeenPlaced)
            {
                orbe.gameObject.transform.position = orbePositions[0].gameObject.transform.position;
                orbe.SetActive(true);
            }
        }

        public void ActivateOrbeOfTheGame(int pos)
        {
            if (!orbeHasAlreadyBeenPlaced)
            {
                orbe.gameObject.transform.position = orbePositions[pos].gameObject.transform.position;
                orbe.SetActive(true);
            }
        }

        public void DeactivateOrbeOfTheGame()
        {
            if (!orbeHasAlreadyBeenPlaced)
            {
                orbe.gameObject.transform.position = orbePositions[0].gameObject.transform.position;
                orbe.SetActive(false);
            }
        }

        public void ActivatePortalsOfTheGame()
        {
            foreach(Portal portal in portalsOfTheGame)
            {
                portal.gameObject.SetActive(true);
            }
        }

        public void PauseGame()
        {
            //State Mechanics / Actions to move within the Finite State Machine
            if (_gameState == GameStates.GAME)
            {
                FinalizeGameState();
                //I should go to pause
                _gameState = GameStates.PAUSE;
                InitializePauseState();
            }
            if (_gameState == GameStates.PAUSE)
            {
                FinalizePauseState();
                //I return to the game
                _gameState = GameStates.GAME;
                InitializeGameState();
            }
        }

        #endregion

        #region GameStateMethods

        #region GameState

        protected void InitializeGameState()
        {
            Time.timeScale = 1.0f;
        }

        protected void ExecutingGameState()
        {
            //TODO: Manage in runtime several aspects of this state
        }

        protected void FinalizeGameState()
        {
            //TODO: Clean or liberate certain variables from this state
        }

        #endregion

        #region PauseState

        protected void InitializePauseState()
        {
            panelPause.SetActive(true);
            Time.timeScale = 0f;
        }

        protected void ExecutingPauseState()
        {
            //TODO: Pending
        }

        protected void FinalizePauseState()
        {
            panelPause.SetActive(false);
            Time.timeScale = 1f;
        }

        #endregion

        #endregion GameStateMethods
    }
}
