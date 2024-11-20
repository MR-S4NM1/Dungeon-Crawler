using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MrSanmi.DungeonCrawler
{
    #region Enum
    public enum playerIndex
    {
        One,
        Two,
        Three,
        Four
    }

    #endregion

    #region Structs
    #endregion
    public class PlayersAvatar : Agent
    {
        #region Knobs

        public playerIndex playerIndex;

        #endregion

        #region References

        [SerializeField] protected HitBox _hitBox;
        [SerializeField] protected GameReferee _gameReferee;

        #endregion

        #region RunTimeVariables

        protected Vector2 _movementInputVector;

        #endregion

        #region LocalMethods

        #endregion

        #region UnityMethods

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            InitializeAgent();
            #endif
        }

        void Update()
        {

        }

        private void FixedUpdate()
        {
            _rb.velocity = _movementInputVector;
        }

        #endregion

        #region PublicMethods

        public void ActivateHitBox()
        {
            _hitBox.ActivateHitBox();
        }

        public void OnMOVE(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                _movementInputVector = value.ReadValue<Vector2>();
                _fsm.SetMovementDirection = _movementInputVector;
                CalculateStateMechanicDirection();
                _fsm.StateMechanic(_stateMechanic);
                _fsm.InitializeState();
            }
            else if (value.canceled)
            {
                _fsm.StateMechanic(StateMechanics.STOP);
                _fsm.FinalizeState();
            }
        }

        public void OnATTACK(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                _fsm.StateMechanic(StateMechanics.ATTACK);
                //PROTOTYPE OF CODE (To be deleted)
                //_hitBox.ActivateHitBox();
            }
            else if (value.canceled)
            {

            }
        }

        public void OnSPRINT(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                CalculateSprintStateMechanicDirection();
                _fsm.StateMechanic(_stateMechanic);
                _fsm.InitializeState();
            }
            else if (value.canceled)
            {
                _fsm.FinalizeState();
                CalculateStateMechanicDirection();
                _fsm.StateMechanic(_stateMechanic);
            }
        }

        public void OnPAUSE(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                _gameReferee.PauseGame();
            }
        }

        public void OnINTERACT(InputAction.CallbackContext value)
        {
            if (value.performed)
            {

            }
            else if (value.canceled)
            {

            }
        }

        public override void InitializeAgent()
        {
            base.InitializeAgent();
            _movementInputVector = new Vector2();
            if(_hitBox == null)
            {
                _hitBox = transform.GetChild(0).gameObject.GetComponent<HitBox>();
            }
        }

        #endregion

        #region GettersSetters
        #endregion
    }
}