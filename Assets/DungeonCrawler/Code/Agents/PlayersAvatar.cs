using MrSanmi.FiniteStateMachine;
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
        [SerializeField] protected GameObject _hitBoxGO;
        [SerializeField] protected GameReferee _gameReferee;
        [SerializeField] protected GameObject _orbeGO;

        #endregion

        #region RunTimeVariables

        protected Vector2 _movementInputVector;
        [SerializeField] protected bool _isInteracting;
        [SerializeField] protected bool _isCarrying;
        [SerializeField] protected bool _isDead;
        [SerializeField] public bool _hasAlreadyBeenActivated;

        #endregion

        #region LocalMethods

        protected virtual void CalculateAttackStateMechanicDirection()
        {
            if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.down) >= 0.5f)
            {
                //DOWN
                _hitBoxGO.transform.position = this.gameObject.transform.position + new Vector3(0.0f, -1.365f, 0.0f);
            }
            else if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.right) >= 0.5f)
            {
                //RIGHT
                _hitBoxGO.transform.position = this.gameObject.transform.position + new Vector3(1.5f, 0.0f, 0.0f);
            }
            else if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.up) >= 0.5f)
            {
                //UP
                _hitBoxGO.transform.position = this.gameObject.transform.position + new Vector3(0.0f, 1.365f, 0.0f);
            }
            else if(Vector2.Dot(_fsm.GetMovementDirection, Vector2.left) >= 0.5f)
            {
                //LEFT
                _hitBoxGO.transform.position = this.gameObject.transform.position + new Vector3(-1.5f, 0.0f, 0.0f);
            }
        }

        #endregion

        #region UnityMethods

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            InitializeAgent();
            #endif
        }
        private void FixedUpdate()
        {
            _rb.velocity = _movementInputVector;
        }

        public void OnEnable()
        {
            UIManager.instance.AddPlayerToSet(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            ValidateOrbeTrigger(other);
        }

        #endregion

        #region LocalMethods

        protected void ValidateOrbeTrigger(Collider2D other)
        {
            if (other.gameObject.CompareTag("Orbe"))
            {
                if (_isInteracting)
                {
                    ActivateOrbe();
                }
            }
        }

        //TODO: Health object validation()

        protected void ActivateOrbe()
        {
            _orbeGO.SetActive(true);
        }

        #endregion

        #region PublicMethods

        public void ActivateHitBox()
        {
            _hitBox.ActivateHitBox();
        }

        public void OnMOVE(InputAction.CallbackContext value)
        {
            if (value.canceled)
            {
                //_fsm.SetMovementDirection = _movementInputVector;
                _fsm.SetMovementSpeed = 0.0f;
                _movementInputVector = Vector2.zero;
                _fsm.SetMovementDirection = Vector2.zero;
                _fsm.StateMechanic(StateMechanics.STOP);
                //_fsm.InitializeState();
            }
            switch (_fsm.GetCurrentState)
            {
                case States.IDLE_DOWN:
                case States.IDLE_RIGHT:
                case States.IDLE_UP:
                case States.IDLE_LEFT:
                case States.MOVING_DOWN:
                case States.MOVING_RIGHT:
                case States.MOVING_UP:
                case States.MOVING_LEFT:
                    if (value.performed)
                    {
                        _movementInputVector = value.ReadValue<Vector2>();
                    }
                    _fsm.SetMovementDirection = _movementInputVector;
                    _fsm.SetMovementSpeed = 5.0f;
                    CalculateStateMechanicDirection();
                    CalculateAttackStateMechanicDirection();
                    _fsm.StateMechanic(_stateMechanic);
                    break;
                case States.SPRINTING_DOWN:
                case States.SPRINTING_RIGHT:
                case States.SPRINTING_UP:
                case States.SPRINTING_LEFT:
                    if (value.performed)
                    {
                        _movementInputVector = value.ReadValue<Vector2>();
                    }
                    _fsm.SetMovementDirection = _movementInputVector;
                    _fsm.SetMovementSpeed = 6.75f;
                    CalculateSprintStateMechanicDirection();
                    CalculateAttackStateMechanicDirection();
                    _fsm.StateMechanic(_stateMechanic);
                    break;
            }

        }

        public void OnATTACK(InputAction.CallbackContext value)
        {
            if(!_isCarrying && !_hitBox.IsHitBoxActivated)
            {
                if (value.performed)
                {
                    CalculateAttackStateMechanicDirection();
                    _fsm.StateMechanic(StateMechanics.ATTACK);
                    //_fsm.InitializeState();
                }
            }
        }

        public void OnSPRINT(InputAction.CallbackContext value)
        {
            if (!_isCarrying)
            {
                switch (_fsm.GetCurrentState)
                {
                    case States.IDLE_DOWN:
                    case States.IDLE_UP:
                    case States.IDLE_RIGHT:
                    case States.IDLE_LEFT:
                        break;
                    case States.MOVING_RIGHT:
                    case States.MOVING_UP:
                    case States.MOVING_DOWN:
                    case States.MOVING_LEFT:
                        if (value.performed)
                        {
                            CalculateSprintStateMechanicDirection();
                            CalculateAttackStateMechanicDirection();
                            _fsm.StateMechanic(_stateMechanic);
                            //_fsm.InitializeState();
                        }
                        break;
                    case States.SPRINTING_DOWN:
                    case States.SPRINTING_RIGHT:
                    case States.SPRINTING_UP:
                    case States.SPRINTING_LEFT:
                        if (value.performed)
                        {
                            CalculateSprintStateMechanicDirection();
                            CalculateAttackStateMechanicDirection();
                            _fsm.StateMechanic(_stateMechanic);
                            //_fsm.InitializeState();
                        }
                        else if (value.canceled)
                        {
                            //_fsm.FinalizeState();
                            CalculateStateMechanicDirection();
                            CalculateAttackStateMechanicDirection();
                            _fsm.StateMechanic(_stateMechanic);
                        }
                        break;
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
                _isInteracting = true;

                if (_isCarrying)
                {
                    _isCarrying = false;
                }
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