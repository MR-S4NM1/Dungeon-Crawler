using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MrSanmi.DungeonCrawler
{
    #region Enum

    public enum States
    {
        //IDLE
        IDLE_DOWN,
        IDLE_RIGHT,
        IDLE_UP,
        IDLE_LEFT,
        //MOVING
        MOVING_DOWN,
        MOVING_RIGHT,
        MOVING_UP,
        MOVING_LEFT,
        //ATTACKING
        ATTACKING_UP,
        ATTACKING_DOWN,
        ATTACKING_LEFT,
        ATTACKING_RIGHT,
        //SPRINTING
        SPRINTING_UP,
        SPRINTING_DOWN,
        SPRINTING_LEFT,
        SPRINTING_RIGHT,
        //INTERACTING
        INTERACTING_UP,
        INTERACTING_DOWN,
        INTERACTING_LEFT,
        INTERACTING_RIGHT,
        //DEATH
        DEATH
    }

    public enum StateMechanics
    {
        //STOP
        STOP,
        //MOVE
        MOVE_UP,
        MOVE_DOWN,
        MOVE_LEFT,
        MOVE_RIGHT,
        //ATTACK
        ATTACK,
        //SPRINTING
        SPRINT_UP,
        SPRINT_DOWN,
        SPRINT_LEFT,
        SPRINT_RIGHT,
        //INTERACTING
        INTERACT,
        //DIE
        DIE //TODO: Complete the code to administrate DIE
    }

    #endregion

    #region Structs
    #endregion

    public class FiniteStateMachine : MonoBehaviour
    {
        #region Knobs
        #endregion

        #region References

        [SerializeField, HideInInspector] protected Animator _animator;
        [SerializeField, HideInInspector] protected Rigidbody2D _rigibody2D;
        [SerializeField, HideInInspector] protected Agent _agent;

        #endregion

        #region RunTimeVariables

        [SerializeField] protected States _state;
        [SerializeField] protected Vector2 _movementDirection;
        [SerializeField] protected float _movementSpeed;

        #endregion

        #region LocalMethods
        protected void InitializeFSM()
        {
            if(_agent == null)
            {
                _agent = GetComponent<Agent>();
            }
        }

        protected void CleanAnimatorFlags()
        {
            foreach(StateMechanics stateMechanic in Enum.GetValues(typeof(StateMechanics)))
            {
                _animator.SetBool(stateMechanic.ToString(), false);
            }
        }

        #endregion

        #region UnityMethods

        private void OnDrawGizmos()
        {
            
        }
        void Start()
        {
            InitializeFSM();
        }

        void FixedUpdate()
        {
            _rigibody2D.velocity = _movementDirection * _movementSpeed;

            ExecutingState();
        }
        #endregion

        #region RuntimeMethods

        public void InitializeState()
        {
            switch (_state)
            {
                case States.IDLE_UP:
                case States.IDLE_DOWN:
                case States.IDLE_LEFT:
                case States.IDLE_RIGHT:
                    InitializeIdleState();
                    break;

                case States.ATTACKING_UP:
                case States.ATTACKING_DOWN:
                case States.ATTACKING_LEFT:
                case States.ATTACKING_RIGHT:
                    InitializeAttackingState();
                    if(_agent as PlayersAvatar)
                    {
                        ((PlayersAvatar)_agent).ActivateHitBox();
                    }
                    break;
                case States.SPRINTING_UP:
                case States.SPRINTING_LEFT:
                case States.SPRINTING_RIGHT:
                case States.SPRINTING_DOWN:
                    InitializeSprintingState();
                    break;
                case States.INTERACTING_UP:
                case States.INTERACTING_LEFT:
                case States.INTERACTING_RIGHT:
                case States.INTERACTING_DOWN:
                    InitializeInteractingState();
                    break;
                case States.DEATH:
                    InitializeDeathState();
                    //gameObject.SetActive(false); //PROTOTYPE TO DELETE
                    break;
            }
        }

        public void ExecutingState()
        {
            switch (_state)
            {
                case States.IDLE_UP:
                case States.IDLE_DOWN:
                case States.IDLE_LEFT:
                case States.IDLE_RIGHT:
                    ExecutingIdleState();
                    break;

                case States.ATTACKING_UP:
                case States.ATTACKING_DOWN:
                case States.ATTACKING_LEFT:
                case States.ATTACKING_RIGHT:
                    ExecutingAttackingState();
                    if (_agent as PlayersAvatar)
                    {
                        ((PlayersAvatar)_agent).ActivateHitBox();
                    }
                    break;
                case States.SPRINTING_UP:
                case States.SPRINTING_LEFT:
                case States.SPRINTING_RIGHT:
                case States.SPRINTING_DOWN:
                    ExecutingSprintingState();
                    break;
                case States.INTERACTING_UP:
                case States.INTERACTING_LEFT:
                case States.INTERACTING_RIGHT:
                case States.INTERACTING_DOWN:
                    ExecutingInteractingState();
                    break;
                case States.DEATH:
                    ExecutingDeathState();
                    break;
            }
        }

        public void FinalizeState()
        {
            switch (_state)
            {
                case States.IDLE_UP:
                case States.IDLE_DOWN:
                case States.IDLE_LEFT:
                case States.IDLE_RIGHT:
                    FinalizeIdleState();
                    break;
                case States.MOVING_UP:
                case States.MOVING_DOWN:
                case States.MOVING_LEFT:
                case States.MOVING_RIGHT:
                    FinalizeMovingState();
                    break;
                case States.ATTACKING_UP:
                case States.ATTACKING_DOWN:
                case States.ATTACKING_LEFT:
                case States.ATTACKING_RIGHT:
                    FinalizeAttackingState();
                    break;
                case States.SPRINTING_UP:
                case States.SPRINTING_LEFT:
                case States.SPRINTING_RIGHT:
                case States.SPRINTING_DOWN:
                    FinalizeSprintingState();
                    break;
                case States.INTERACTING_UP:
                case States.INTERACTING_LEFT:
                case States.INTERACTING_RIGHT:
                case States.INTERACTING_DOWN:
                    FinalizeInteractingState();
                    break;
                case States.DEATH:
                    FinalizeDeathState();
                    break;
            }
        }

        #endregion

        #region PublicMethods

        // Action
        public void StateMechanic(StateMechanics value)
        {
            _animator.SetBool(value.ToString() , true);
        }

        public void SetState(States value)
        {
            CleanAnimatorFlags();
            _state = value;
            InitializeState();
        }

        #endregion

        #region GettersSetters

        public Vector2 GetMovementDirection
        {
            get { return _movementDirection; }
        }

        public Vector2 SetMovementDirection
        {
            set { _movementDirection = value; }
        }

        public float SetMovementSpeed
        {
            set { _movementSpeed = value; }
        }

        #endregion

        #region FiniteStateMachineStates

        #region IdleState
        protected virtual void InitializeIdleState()
        {
            _movementSpeed = 10.0f;
        }

        protected virtual void ExecutingIdleState()
        {

        }
        protected virtual void FinalizeIdleState()
        {

        }
        #endregion IdleState

        #region MovingState
        protected virtual void InitializeMovingState()
        {

        }

        protected virtual void ExecutingMovingState()
        {

        }
        protected virtual void FinalizeMovingState()
        {
            _rigibody2D.velocity = Vector2.zero;
            _movementDirection = Vector2.zero;
            _movementSpeed = 0.0f;
        }
        #endregion MovingState

        #region AttackingState
        protected virtual void InitializeAttackingState()
        {

        }

        protected virtual void ExecutingAttackingState()
        {

        }
        protected virtual void FinalizeAttackingState()
        {

        }
        #endregion AttackingState

        #region SprintingState
        protected virtual void InitializeSprintingState()
        {
            _movementSpeed *= 1.25f;
        }

        protected virtual void ExecutingSprintingState()
        {

        }
        protected virtual void FinalizeSprintingState()
        {
            _movementSpeed *= 0.75f;
        }
        #endregion SprintingState

        #region InteractingState
        protected virtual void InitializeInteractingState()
        {

        }

        protected virtual void ExecutingInteractingState()
        {

        }
        protected virtual void FinalizeInteractingState()
        {

        }
        #endregion InteractingState

        #region DeathState
        protected virtual void InitializeDeathState()
        {

        }

        protected virtual void ExecutingDeathState() 
        {

        }
        protected virtual void FinalizeDeathState()
        {

        }
        #endregion InteractingState

        #endregion FiniteStateMachineStates
    }
}