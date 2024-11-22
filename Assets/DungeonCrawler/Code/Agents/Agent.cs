using MrSanmi.DungeonCrawler;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        public int maxHealthPoints = 3;
        public float cooldownDamageTime = 1f;


        #endregion

        #region References

        [SerializeField, HideInInspector] protected Rigidbody2D _rb;
        [SerializeField, HideInInspector] protected FiniteStateMachine _fsm;
        [SerializeField] protected HurtBox _hurtBox;
        [SerializeField] protected SpriteRenderer _spriteRenderer;

        #endregion

        #region RunTimeVariables

        protected Vector2 _movementDirection;
        protected StateMechanics _stateMechanic;
        [SerializeField] protected Color _spriteRendererColor;
        protected bool _cooldown;
        protected float _cooldownCronometer;
        protected Coroutine _blinkDamgeCoroutine;



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

        //Invocation from the HurtBox of this same agent
        protected virtual void DamageAgent()
        {
            if (!_cooldown)
            {
                _cooldown = true;
                _cooldownCronometer = cooldownDamageTime;
                _blinkDamgeCoroutine = StartCoroutine(DamageBlink());
            }
        }

        protected IEnumerator DamageBlink()
        {
            while (_cooldownCronometer > 0)
            {
                _spriteRendererColor.a = 0.25f;
                _spriteRenderer.color = _spriteRendererColor;
                yield return new WaitForSeconds(0.25f);
                _spriteRendererColor.a = 1f;
                _spriteRenderer.color = _spriteRendererColor;
                yield return new WaitForSeconds(0.25f);
                _cooldownCronometer -= 0.5f; //0.25f + 0.25f
            }
            StopCoroutine(_blinkDamgeCoroutine);
            _blinkDamgeCoroutine = null;
            _cooldown = false;
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

            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
            if (_spriteRendererColor == null)
            {
                _spriteRendererColor = _spriteRenderer.color;
            }
            //With the RequireComponent we guarantee
            //this reference will be ALWAYS retreived
            /*
            _rigidbody = GetComponent<Rigidbody2D>();
            if (_rigidbody == null ) {
                Debug.LogError("Rigid body has not been assigned to " +
                    gameObject.name);
            }
            */
        }

        public void GoToDeathState()
        {
            if(_hurtBox.CurrentHealthPoints <= 0)
            {
                StateMechanic(StateMechanics.DIE);
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