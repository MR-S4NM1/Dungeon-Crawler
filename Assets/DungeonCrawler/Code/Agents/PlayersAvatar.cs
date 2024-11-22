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
        [SerializeField] protected bool _isInteracting;
        [SerializeField] protected bool _isCarrying;

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
            }
            else if (value.canceled)
            {
                _fsm.StateMechanic(StateMechanics.STOP);
                _fsm.InitializeState();
            }
        }

        public void OnATTACK(InputAction.CallbackContext value)
        {
            if(!_isCarrying)
            {
                if (value.performed)
                {
                    _fsm.StateMechanic(StateMechanics.ATTACK);
                    _fsm.InitializeState();
                }
            }
        }

        public void OnSPRINT(InputAction.CallbackContext value)
        {
            if (!_isCarrying)
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
                _isCarrying = false;
                _isInteracting = true;
            }
            else if (value.canceled)
            {
                _isInteracting = false;
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

        public bool GetIsInteracting
        {
            get { return _isInteracting; }
        }

        public bool SetIsCarrying
        {
            set { _isCarrying = value; }
        }

        #endregion
    }
}