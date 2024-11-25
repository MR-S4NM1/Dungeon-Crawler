using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrSanmi.DungeonCrawler
{
    #region Enum

    public enum EnemyBehavioursState
    {
        PATROL,
        PERSECUTION,
        SHOOT
    }

    public enum EnemyType 
    {
        PATROLLING,
        PATROLLING_AND_PERSECUTION,
        PATROLLING_AND_SHOOTING
    }

    #endregion

    #region Structs
    #endregion
    public class EnemyNPC : Agent
    {
        #region Knobs

        public EnemyBehaviours_ScriptableObject scriptBehaviours;
        [SerializeField] protected EnemyType _enemyType;

        #endregion

        #region References

        [SerializeField] protected EnemyPool _enemyPool;

        #endregion

        #region RunTimeVariables

        [SerializeField] protected EnemyBehaviour _currentEnemyBehaviour;
        protected EnemyBehavioursState _currentEnemyBehaviourState;
        protected int _currentEnemyBehaviourIndex;
        protected Transform _avatarsTransform;
        protected StateMechanics _previousMovementStateMechanic;

        #endregion

        #region LocalMethods

        protected void InvokeStateMechanic()
        {
            switch (_currentEnemyBehaviour.type) {
                case EnemyBehaviourType.STOP:
                    _fsm.StateMechanic(StateMechanics.STOP);
                    break;
                case EnemyBehaviourType.FIRE:
                    //
                    break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                    //
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    _fsm.StateMechanic(_stateMechanic);
                    break;
            }
        }

        protected IEnumerator TimerForEnemyBehaviour() 
        {
            yield return new WaitForSeconds(_currentEnemyBehaviour.time);
            FinalizeSubState();
            GoToNextEnemyBehaviour();
        }

        protected void GoToNextEnemyBehaviour()
        {
            _currentEnemyBehaviourIndex++;
            if(_currentEnemyBehaviourState == EnemyBehavioursState.PATROL)
            {
                if(_currentEnemyBehaviourIndex >= scriptBehaviours.patrolBehaviours.Length)
                {
                    _currentEnemyBehaviourIndex = 0;
                }
                _currentEnemyBehaviour = scriptBehaviours.patrolBehaviours[_currentEnemyBehaviourIndex];
            }
            else // PERSECUTING
            {
                if (_currentEnemyBehaviourIndex >= scriptBehaviours.persecutionBehaviours.Length)
                {
                    _currentEnemyBehaviourIndex = 0;
                }
                _currentEnemyBehaviour = scriptBehaviours.persecutionBehaviours[_currentEnemyBehaviourIndex];
            }
            InitializeSubState();
            CalculateStateMechanicDirection();
            InvokeStateMechanic();
            if (_currentEnemyBehaviour.time > 0)
            {
                StartCoroutine(TimerForEnemyBehaviour());
            }
        }

        protected void InitializeSubState()
        {
            switch (_currentEnemyBehaviour.type)
            {
                case EnemyBehaviourType.STOP:
                    InitializeStopSubStateMachine();
                    break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                    InitializeMoveToRandomDirectionSubStateMachine();
                    break;
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    InitializePersecuteTheAvatarSubStateMachine();
                    break;
            }
        }

        protected void FinalizeSubState()
        {
            switch (_currentEnemyBehaviour.type)
            {
                case EnemyBehaviourType.STOP:
                    FinalizeStopSubStateMachine();
                    break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                    FinalizeMoveToRandomDirectionSubStateMachine();
                    break;
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    FinalizePersecuteTheAvatarSubStateMachine();
                    break;
            }
        }

        protected void InitializePersecutionBehaviour()
        {
            StopAllCoroutines();
            _currentEnemyBehaviourState = EnemyBehavioursState.PERSECUTION;
            _currentEnemyBehaviourIndex = 0;

            if (scriptBehaviours.persecutionBehaviours.Length > 0)
            {
                _currentEnemyBehaviour = scriptBehaviours.persecutionBehaviours[0];
            }
            else
            {
                _currentEnemyBehaviour.type = EnemyBehaviourType.PERSECUTE_THE_AVATAR;
                _currentEnemyBehaviour.time = -1;
                _currentEnemyBehaviour.speed = 1.0f;
            }
            InitializeSubState();
            CalculateStateMechanicDirection();
            InvokeStateMechanic();
            if (_currentEnemyBehaviour.time > 0)
            {
                StartCoroutine(TimerForEnemyBehaviour());
            }
        }

        protected void InitializePatrolBehaviour()
        {
            StopAllCoroutines();
            _currentEnemyBehaviourState = EnemyBehavioursState.PATROL;
            _currentEnemyBehaviourIndex = 0;

            if (scriptBehaviours.patrolBehaviours.Length > 0)
            {
                _currentEnemyBehaviour = scriptBehaviours.patrolBehaviours[0];
            }
            else
            {
                _currentEnemyBehaviour.type = EnemyBehaviourType.STOP;
                _currentEnemyBehaviour.time = -1;
            }
            InitializeSubState();
            CalculateStateMechanicDirection();
            InvokeStateMechanic();
            if (_currentEnemyBehaviour.time > 0)
            {
                StartCoroutine(TimerForEnemyBehaviour());
            }
        }

        #endregion

        #region UnityMethods
        void Start()
        {
            InitializeAgent();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void OnEnable()
        {
            InitializePatrolBehaviour();
        }

        void FixedUpdate()
        {
            switch (_currentEnemyBehaviour.type)
            {
                case EnemyBehaviourType.STOP:
                    ExecutingStopSubStateMachine();
                    break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                    ExecutingMoveToRandomDirectionSubStateMachine();
                    break;
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    ExecutingPersecuteTheAvatarSubStateMachine();
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (_enemyType)
            {
                case EnemyType.PATROLLING_AND_PERSECUTION:
                    if (other.CompareTag("Player"))
                    {
                        _avatarsTransform = other.gameObject.transform;
                        InitializePersecutionBehaviour();
                    }
                    break;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            switch (_enemyType)
            {
                case EnemyType.PATROLLING_AND_PERSECUTION:
                    if (other.CompareTag("Player"))
                    {
                        _avatarsTransform = null;
                        InitializePatrolBehaviour(); // Initialize patrol
                    }
                    break;
            }
        }

        #endregion

        #region PublicMethods

        public override void InitializeAgent()
        {
           InitializePatrolBehaviour();
        }

        public virtual void AlertPoolAboutDeath()
        {
            _enemyPool?.SubstractEnemyFromEnemyCount();
        }

        public virtual void ApplyForce(Vector2 direction)
        {
            print("Forceeeeeeee!" + direction);
            _rb.AddForce(direction * 5.0f, ForceMode2D.Impulse);
        }

        #endregion

        #region GettersSetters
        #endregion

        #region SubStateMachineStates

        #region StopSubStateMachineMethods

        protected void InitializeStopSubStateMachine()
        {
            _fsm.SetMovementSpeed = 0.0f;
            _fsm.SetMovementDirection = Vector2.zero;
        }

        protected void ExecutingStopSubStateMachine()
        {
            // Do nothing
        }

        protected void FinalizeStopSubStateMachine()
        {
            // Do nothing
        }

        #endregion StopSubStateMachineMethods

        #region MoveToRandomDirectionSubStateMachineMethods

        protected void InitializeMoveToRandomDirectionSubStateMachine()
        {
            do
            {
                _movementDirection =
                    new Vector2(
                        UnityEngine.Random.Range(-1.0f, 1.0f),
                        UnityEngine.Random.Range(-1.0f, 1.0f)
                    ).normalized;
            } while (_movementDirection.magnitude == 0.0f);

            _fsm.SetMovementDirection = _movementDirection;
            _fsm.SetMovementSpeed = _currentEnemyBehaviour.speed;
        }

        protected void ExecutingMoveToRandomDirectionSubStateMachine()
        {
            // Do nothing
        }

        protected void FinalizeMoveToRandomDirectionSubStateMachine()
        {
            _fsm.SetMovementDirection = Vector2.zero;
            _fsm.SetMovementSpeed = 0.0f;
        }

        #endregion MoveToRandomDirectionSubStateMachineMethods

        #region PersecuteTheAvatarSubStateMachineMethods

        protected void InitializePersecuteTheAvatarSubStateMachine()
        {
            _fsm.SetMovementDirection = (_avatarsTransform.position - transform.position).normalized;
            _fsm.SetMovementSpeed = _currentEnemyBehaviour.speed;
            _previousMovementStateMechanic = _stateMechanic;
        }

        protected void ExecutingPersecuteTheAvatarSubStateMachine()
        {
            // as the avatar may move, we have to update the direction towards him or her
            _fsm.SetMovementDirection = (_avatarsTransform.position - transform.position).normalized;
            CalculateStateMechanicDirection();
            if(_previousMovementStateMechanic != _stateMechanic)
            {
                InvokeStateMechanic();
                _previousMovementStateMechanic = _stateMechanic;
            }
        }

        protected void FinalizePersecuteTheAvatarSubStateMachine()
        {
            _fsm.SetMovementSpeed = 0.0f;
            _fsm.SetMovementDirection = Vector2.zero;
        }

        #endregion PersecuteAvatarSubStateMachineMethods

        #region ShootToAvatarSubStateMachineMethods

        protected void InitializeShootToAvatarSubStateMachine()
        {

        }

        protected void ExecutingShootToAvatarSubStateMachine()
        {

        }

        protected void FinalizeShootToAvatarSubStateMachine()
        {

        }

        #endregion

        #endregion SubStateMachineStates
    }
}