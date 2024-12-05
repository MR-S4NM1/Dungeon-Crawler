using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEditorInternal;

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
        //DEATH
        DEATH
    }

    public enum StateMechanics
    {
        //STOP
        STOP,
        //MOVE
        MOVE_DOWN,
        MOVE_RIGHT,
        MOVE_UP,
        MOVE_LEFT,
        //ATTACK
        ATTACK,
        //SPRINTING
        SPRINT_DOWN,
        SPRINT_RIGHT,
        SPRINT_UP,
        SPRINT_LEFT,
        //DIE
        DIE, //TODO: Complete the code to administrate DIE
        //REVIVE (JUST FOR ENEMIES OF A POOL)
        REVIVE
    }

    #endregion

    #region Structs
    #endregion

    public class FiniteStateMachine : MonoBehaviour
    {
        #region Knobs
        #endregion

        #region References

        [SerializeField] protected Animator _animator;
        [SerializeField] protected Rigidbody2D _rigibody2D;
        [SerializeField] protected Agent _agent;
        [SerializeField] protected EnemyNPC _enemyNPC;
        [SerializeField] protected DestroyableObject _destroyableObject;
        [SerializeField] protected PlayersAvatar _playersAvatar;
        [SerializeField] protected AnimationClip _deathAnimationClip;

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
            //The agent received a force impulse greater than the input:
            //if (_rigibody2D.velocity.magnitude > _movementSpeed)
            //{
            //    //_rigibody2D.velocity += (_movementDirection * _movementSpeed);
            //}
            //else //the agent is moving normally according to the movement input or not
            //{
            _rigibody2D.velocity = (_movementDirection * _movementSpeed);
            //}
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
                case States.MOVING_UP:
                case States.MOVING_DOWN:
                case States.MOVING_LEFT:
                case States.MOVING_RIGHT:
                    InitializeMovingState();
                    break;
                case States.ATTACKING_UP:
                case States.ATTACKING_DOWN:
                case States.ATTACKING_LEFT:
                case States.ATTACKING_RIGHT:
                    InitializeAttackingState();
                    break;
                case States.SPRINTING_UP:
                case States.SPRINTING_LEFT:
                case States.SPRINTING_RIGHT:
                case States.SPRINTING_DOWN:
                    InitializeSprintingState();
                    break;
                case States.DEATH:
                    InitializeDeathState();
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
                    break;
                case States.SPRINTING_UP:
                case States.SPRINTING_LEFT:
                case States.SPRINTING_RIGHT:
                case States.SPRINTING_DOWN:
                    ExecutingSprintingState();
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

        public States GetCurrentState
        {
            get { return _state; }
        }

        #endregion

        #region FiniteStateMachineStates

        #region IdleState
        protected virtual void InitializeIdleState()
        {
            if(_agent as PlayersAvatar)
            {
                _movementSpeed = 0.0f;
            }
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
            if (_agent as PlayersAvatar)
            {
                _movementSpeed = 5.0f;
            }
        }

        protected virtual void ExecutingMovingState()
        {

        }
        protected virtual void FinalizeMovingState()
        {

        }
        #endregion MovingState

        #region AttackingState
        protected virtual void InitializeAttackingState()
        {
            if (_agent as PlayersAvatar)
            {
                ((PlayersAvatar)_agent)?.ActivateHitBox();
                _movementSpeed = 0.0f;
            }
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
            if (_agent as PlayersAvatar)
            {
                _movementSpeed = 6.75f;
            }
        }

        protected virtual void ExecutingSprintingState()
        {

        }
        protected virtual void FinalizeSprintingState()
        {

        }
        #endregion SprintingState

        #region DeathState
        protected virtual void InitializeDeathState()
        {
            StartCoroutine(DeathCoroutine());
        }

        protected virtual void ExecutingDeathState() 
        {

        }
        protected virtual void FinalizeDeathState()
        {

        }

        IEnumerator DeathCoroutine()
        {
            _movementDirection = Vector2.zero;
            _movementSpeed = 0.0f;
            //((DestroyableObject)_agent)?.InstantiateObject();
            _destroyableObject?.InstantiateObject();
            yield return new WaitForSeconds(_deathAnimationClip.length);

            switch (_agent)
            {
                case PlayersAvatar:
                    //((PlayersAvatar)_agent)?.DeactivateHitAndHurtBoxesWhenDying(); //This didn't work, believe me ;-;
                    //((PlayersAvatar)_agent)?.AlertGameRefereeAboutMyDeath();
                    _playersAvatar?.DeactivateHitAndHurtBoxesWhenDying();
                    _playersAvatar?.AlertGameRefereeAboutMyDeath();
                    break;
                case EnemyNPC:
                    //((EnemyNPC)_agent)?.AlertPoolAboutDeath();
                    _enemyNPC?.AlertPoolAboutDeath();
                    gameObject.SetActive(false);
                    break;
                case DestroyableObject:
                    gameObject.SetActive(false);
                    break;
            }
        }

        #endregion InteractingState

        #endregion FiniteStateMachineStates
    }
}