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

        [Header("UI Panels")]
        [SerializeField] public GameObject pausePanel;
        [SerializeField] public GameObject gamePanel;

        [Header("Target Group")]
        [SerializeField] public CinemachineTargetGroup targetGroup;

        [Header("Interactive Objects")]
        [SerializeField] protected GameObject chest;
        [SerializeField] protected GameObject pedestal;

        [Header("Path Locks")]
        [SerializeField] protected GameObject[] finalPathLocks;
        [SerializeField] protected GameObject[] middlePathLocks;

        [Header("Portal")]
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
        private void OnEnable()
        {
            Time.timeScale = 1.0f;
        }

        private void OnDisable()
        {
            Time.timeScale = 1.0f;
        }


        void Start() //Runtime
        {
            Time.timeScale = 1.0f;
            _gameState = GameStates.GAME;
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
                    ExecutingGameOverState();
                    break;
                case GameStates.VICTORY:
                    ExecutingVictoryState();
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
                    for (int i = 0; i < middlePathLocks.Length; ++i)
                    {
                        StartCoroutine(MiddlePathLockReleaseCorroutine(i));
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

        public void AddPlayerToCinemachineTargetGroup(Transform playerTransform)
        {
            targetGroup.AddMember(playerTransform, 1 , 0);
        }

        public void RemoveAndSubstractPlayerFromTheCounter(Transform playersTransform)
        {
            targetGroup.RemoveMember(playersTransform);

            if (targetGroup.m_Targets.Length <= 0)
            {
                LoseGame();
            }
        }

        public void PauseGame()
        {
            //State Mechanics / Actions to move within the Finite State Machine
            if (_gameState == GameStates.GAME)
            {
                FinalizeGameState();
                //I should go to pause
                InitializePauseState();
                _gameState = GameStates.PAUSE;
            }
            else if (_gameState == GameStates.PAUSE)
            {
                FinalizePauseState();
                //I return to the game
                InitializeGameState();
                _gameState = GameStates.GAME;
            }
        }

        public void WinGame()
        {
            //State Mechanics / Actions to move within the Finite State Machine
            if (_gameState == GameStates.GAME)
            {
                FinalizeGameState();
                InitializeVictoryState();
                _gameState = GameStates.VICTORY;
            }
        }

        public void LoseGame()
        {
            if (_gameState == GameStates.GAME)
            {
                FinalizeGameState();
                _gameState = GameStates.GAME_OVER;
                InitializeGameOverState();
            }
        }

        #endregion

        #region GameStateMethods

        #region GameState

        protected void InitializeGameState()
        {
            Time.timeScale = 1.0f;
            gamePanel?.SetActive(true);
            pausePanel?.SetActive(false);
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
            gamePanel?.SetActive(false);
            pausePanel?.SetActive(true);
            Time.timeScale = 0.0f;
        }

        protected void ExecutingPauseState()
        {

        }

        protected void FinalizePauseState()
        {
            gamePanel?.SetActive(true);
            pausePanel?.SetActive(false);
            Time.timeScale = 1.0f;
        }

        #endregion

        #region VictoryState

        protected void InitializeVictoryState()
        {
            SceneChanger.instance.ChangeSceneTo(2); // 2 -> Victory Scene :D
        }

        protected void ExecutingVictoryState()
        {

        }

        protected void FinalizeVictoryState()
        {

        }

        #endregion

        #region GameOverState

        protected void InitializeGameOverState()
        {
            SceneChanger.instance.ChangeSceneTo(3); // 3 -> Game Over Scene :(
        }

        protected void ExecutingGameOverState()
        {

        }

        protected void FinalizeGameOverState()
        {

        }

        #endregion

        #endregion GameStateMethods

        #region Corroutines

        public IEnumerator MiddlePathLockReleaseCorroutine(int index)
        {
            middlePathLocks[index].GetComponent<Animator>().Play("PathLockRelease");

            yield return new WaitForSeconds(3.0f);

            middlePathLocks[index].GetComponent<BoxCollider2D>().enabled = false;
        }

        #endregion
    }
}
