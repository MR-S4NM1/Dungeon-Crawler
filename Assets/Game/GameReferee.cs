using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

        [SerializeField] protected GameObject chest;
        [SerializeField] protected GameObject pedestal;

        [SerializeField] protected GameObject[] finalPathLocks;
        [SerializeField] protected GameObject[] middlePathLocks;
        [SerializeField] protected Portal[] portalsOfTheGame;

        #endregion

        #region RuntimeVariables

        [SerializeField] protected GameStates _gameState;
        [SerializeField] public bool orbeHasAlreadyBeenPlaced;
        [SerializeField] public bool aPlayerIsCarryingTheOrbe;
        [SerializeField] public bool orbeIsInChest;
        [SerializeField] public bool orbeIsInPedestal;

        #endregion

        #region UnityMethods
        void Start() //Runtime
        {
            InitializeGameState();
            orbeHasAlreadyBeenPlaced = false;
            aPlayerIsCarryingTheOrbe = false;
            orbeIsInChest = true;
            orbeIsInPedestal = false;
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

        public void ActivateChestOrbeAndOpenChest()
        {
            if (!orbeHasAlreadyBeenPlaced)
            {
                chest.GetComponent<InteractiveStuff>().OpenChest();
            }
        }

        public void ActivatePedestalOrbe()
        {
            if (!orbeHasAlreadyBeenPlaced)
            {
                pedestal.transform.GetChild(0).gameObject.SetActive(true);
                DeactivatePathLocks("FinalPathLocks");
                orbeHasAlreadyBeenPlaced = true;
                orbeIsInPedestal = true;
            }
        }

        public void DeactivateChestOrbeAndCloseChest()
        {
            if (!orbeHasAlreadyBeenPlaced)
            {
                chest.GetComponent<InteractiveStuff>().CloseChest();
                orbeIsInChest = true;
            }
        }

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

        }

        protected void FinalizeGameState()
        {
            //TODO: Clean or liberate certain variables from this state
        }

        #endregion

        #region PauseState

        protected void InitializePauseState()
        {
            panelPause?.SetActive(true);
            Time.timeScale = 0.0f;
        }

        protected void ExecutingPauseState()
        {

        }

        protected void FinalizePauseState()
        {
            panelPause?.SetActive(false);
            Time.timeScale = 1.0f;
        }

        #endregion

        #endregion GameStateMethods
    }
}
