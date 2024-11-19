using MrSanmi.DungeonCrawler;
using MrSanmi.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrSanmi.DungeonCrawler
{
    #region Enum
    #endregion

    #region Structs
    #endregion

    //[RequireComponent(typeof(Rigidbody2D))]
    public class Agent : MonoBehaviour
    {
        #region Knobs
        #endregion

        #region References

        [SerializeField, HideInInspector] protected Rigidbody2D _rb;
        [SerializeField, HideInInspector] protected FiniteStateMachine _fsm;

        #endregion

        #region RunTimeVariables

        protected Vector2 _movementDirection;
        protected StateMechanics _stateMechanic;

        #endregion

        #region LocalMethods

        protected virtual void CalculateStateMechanicDirection()
        {
            if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.down) >= 0.5f)
            {
                _stateMechanic = StateMechanics.MOVE_DOWN;
            }
            else if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.right) >= 0.5f) 
            {
                _stateMechanic = StateMechanics.MOVE_RIGHT;
            }
            else if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.up) >= 0.5f)
            {
                _stateMechanic = StateMechanics.MOVE_UP;
            }
            else
            {
                _stateMechanic = StateMechanics.MOVE_LEFT;
            }
        }

        protected virtual void CalculateSprintStateMechanicDirection()
        {
            if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.down) >= 0.5f)
            {
                _stateMechanic = StateMechanics.SPRINT_DOWN;
            }
            else if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.right) >= 0.5f)
            {
                _stateMechanic = StateMechanics.SPRINT_RIGHT;
            }
            else if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.up) >= 0.5f)
            {
                _stateMechanic = StateMechanics.SPRINT_UP;
            }
            else
            {
                _stateMechanic = StateMechanics.SPRINT_LEFT;
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

        //void Start()
        //{
        //   InitializeAgent();
        //}

        void Update()
        {

        }
        #endregion

        #region PublicMethods
        public virtual void InitializeAgent()
        {
            _rb = GetComponent<Rigidbody2D>();
            if (_rb == null)
            {
                Debug.LogError("Rigid body has not been assigned to " + gameObject.name);
            }
        }

        public void StateMechanic(StateMechanics value)
        {
            _fsm.StateMechanic(value);
        }
        #endregion

        #region GettersSetters
        #endregion
    }
}