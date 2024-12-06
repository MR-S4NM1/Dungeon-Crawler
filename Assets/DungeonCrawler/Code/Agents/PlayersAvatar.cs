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
        ONE,
        TWO,
        THREE,
        FOUR
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

        #endregion

        #region RunTimeVariables

        protected Vector2 _movementInputVector;
        [SerializeField] protected bool _isInteracting;
        [SerializeField] protected bool _isCarrying;
        [SerializeField] protected bool _isDead;
        [SerializeField] public bool _hasAlreadyBeenActivated;
        [SerializeField] public bool _isInteractingWithPedestal;
        [SerializeField] public bool _isInteractingWithChest;
        [SerializeField] public bool _isInteractingWithOrbe;

        [SerializeField] protected Transform _pedestalTransform;
        [SerializeField] protected Transform _chestTransform;
        [SerializeField] protected GameObject _orbe;


        #endregion

        #region UnityMethods

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            InitializeAgent();
            #endif
        }

        private void Start()
        {
            _chestTransform = null;
            _pedestalTransform = null;
            _orbe = null;
        }

        private void FixedUpdate()
        {
            _rb.velocity = _movementInputVector;

            if (_isCarrying && (_orbe != null))
            {
                _orbe.transform.localPosition = new Vector3(0, 1.5f, 0);

                //I tried to NOT do this, but due to an unknown reason, my booleans DIDN'T change when I called my functions ;-;
                _gameReferee.aPlayerIsCarryingTheOrbe = true;
                _gameReferee.orbeIsInChest = false;
            }
        }

        public void OnEnable()
        {
            _fsm.SetState(States.IDLE_DOWN);
            UIManager.instance.AddPlayerToDictionary(this, playerIndex);
            _gameReferee.AddPlayerToCinemachineTargetGroup(this.gameObject.transform);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            ValidateOrbePedestalOrChestTriggerEnter(other);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            ValidateOrbePedestalOrChestTriggerExit(collision);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            ValidateHealthCollision(other);
        }

        #endregion

        #region LocalMethods

        protected virtual void CalculateAttackStateMechanicDirection()
        {
            if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.down) >= 0.5f)
            {
                //DOWN
                _hitBoxGO.transform.position = this.gameObject.transform.position + new Vector3(0.0f, -1.2f, 0.0f);
            }
            else if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.right) >= 0.5f)
            {
                //RIGHT
                _hitBoxGO.transform.position = this.gameObject.transform.position + new Vector3(1.1f, -0.3f, 0.0f);
            }
            else if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.up) >= 0.5f)
            {
                //UP
                _hitBoxGO.transform.position = this.gameObject.transform.position + new Vector3(0.0f, 0.75f, 0.0f);
            }
            else if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.left) >= 0.5f)
            {
                //LEFT
                _hitBoxGO.transform.position = this.gameObject.transform.position + new Vector3(-1.1f, -0.3f, 0.0f);
            }
        }

        protected void ValidateOrbePedestalOrChestTriggerEnter(Collider2D other)
        {
            if (other.gameObject.CompareTag("Pedestal"))
            {
                _isInteractingWithPedestal = true;
                _pedestalTransform = other.transform;

                if (_isInteracting)
                {
                    other.gameObject.GetComponent<InteractiveStuff>().pedestalBool = true;
                }
            }
            else if (other.gameObject.CompareTag("Orbe"))
            {
                _orbe = other.gameObject;
                _isInteractingWithOrbe = true;
            }
            else if (other.gameObject.CompareTag("Chest"))
            {
                _isInteractingWithChest = true;
                _chestTransform = other.transform;

                if (_isInteracting)
                {
                    other.gameObject.GetComponent<InteractiveStuff>().chestBool = true;
                }
            }
        }

        protected void ValidateOrbePedestalOrChestTriggerExit(Collider2D other)
        {
            if (other.gameObject.CompareTag("Pedestal"))
            {
                _isInteractingWithPedestal = false;
            }
            else if (other.gameObject.CompareTag("Chest"))
            {
                _isInteractingWithChest = false;
            }
            else if (other.gameObject.CompareTag("Orbe"))
            {
                _isInteractingWithOrbe = false;
            }
        }

        protected void ValidateHealthCollision(Collision2D other)
        {
            if (other.gameObject.CompareTag("Health"))
            {
                if(_hurtBox._currentHealthPoints < maxHealthPoints)
                {
                    _hurtBox._currentHealthPoints++;
                    //UIManager.instance.UpdateHeartUIIcons(this, _hurtBox._currentHealthPoints, "Add");
                    UpdateUIHealth();
                }
                other.gameObject.SetActive(false);
            }
        }

        protected void PickUpOrbe(GameObject orbe)
        {
            if (orbe != null)
            {
                orbe.transform.parent = null;
                orbe.transform.parent = this.gameObject.transform;
                orbe.transform.localPosition = new Vector3(0, 1.5f, 0);
                _isCarrying = true;
                _gameReferee.DeactivateChestOrbeAndCloseChest();
                _gameReferee.aPlayerIsCarryingTheOrbe = true;
                _gameReferee.orbeIsInChest = false;
            }
        }

        protected void PlaceOrbeOnPedestal(GameObject orbe, Transform pedestal)
        {
            if ((orbe != null) && (pedestal != null))
            {
                orbe.transform.SetParent(null);
                orbe.transform.position = pedestal.position;
                _gameReferee.ActivatePedestalOrbe();
                _isCarrying = false;
                _gameReferee.orbeIsInPedestal = true;
                _gameReferee.orbeIsInChest = false;
                _orbe = null;
                _pedestalTransform = null;
                _chestTransform = null;
            }
        }

        protected void ReturnOrbeToChest(GameObject orbe, Transform chest)
        {
            if ((orbe != null) && (chest != null))
            {
                orbe.transform.SetParent(null);
                orbe.transform.parent = _chestTransform;
                orbe.transform.position = _chestTransform.position;
                _gameReferee.DeactivateChestOrbeAndCloseChest();
                _isCarrying = false;
                _gameReferee.aPlayerIsCarryingTheOrbe = false;
                _gameReferee.orbeIsInChest = true;
            }
        }

        #endregion

        #region PublicMethods

        public void ReturnOrbeToTheChestPublic()
        {
            ReturnOrbeToChest(_orbe, _chestTransform);
        }

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
                print("YESSSSSSSSSSSSSSSSSSSSSSSSSSS");
                _gameReferee.PauseGame();
            }
        }

        public void OnINTERACT(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                _isInteracting = true;

                //I DON'T USE ELSE IF, BECAUSE IT WON'T READ THE CONDITIONAL IS THE PREVIOUS ONE IS ACCEPTED ;-;
                if (_isInteracting && _isInteractingWithChest && !_isCarrying && !_gameReferee.aPlayerIsCarryingTheOrbe)
                {
                    _gameReferee.ActivateChestOrbeAndOpenChest();
                }
                if (_isInteracting && _isInteractingWithOrbe && _isInteractingWithChest && !_isCarrying)
                {
                    PickUpOrbe(_orbe);
                }
                if (_isInteracting && _isInteractingWithPedestal && _isCarrying)
                {
                    PlaceOrbeOnPedestal(_orbe, _pedestalTransform);
                }
                if (_isInteracting && _isCarrying && !_isInteractingWithPedestal && !_isInteractingWithChest)
                {
                    ReturnOrbeToChest(_orbe, _chestTransform);
                }

                _isInteracting = false;
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

        public void AlertGameRefereeAboutMyDeath()
        {
            _gameReferee.RemoveAndSubstractPlayerFromTheCounter(this.gameObject.transform);
        }

        public void DeactivateHitAndHurtBoxesWhenDying()
        {
            _hurtBox.gameObject.SetActive(false);
            _hitBox.gameObject.SetActive(false);
        }

        public void UpdateUIHealth()
        {
            //UIManager.instance.UpdateHeartUIIcons(this, _hurtBox._currentHealthPoints, "Substract");
            UIManager.instance.UpdateHeartUIIcons(this, _hurtBox._currentHealthPoints);
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