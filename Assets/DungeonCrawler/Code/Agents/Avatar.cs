using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MrSanmi.FiniteStateMachine
{
    #region Enum
    public enum Index
    {
        ONE, 
        TWO, 
        THREE, 
        FOUR
    }
    #endregion

    public class Avatar : MonoBehaviour
    {
        #region Knobs
        public Index playerIndex;
        #endregion

        #region RunTimeVariables
        protected Rigidbody2D rb;
        protected Vector3 direction;
        #endregion

        #region UnityMethods

        void Start()
        {
            rb= GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            rb.velocity = direction;
        }
        #endregion

        #region InputCallbackthMethods
        public void OnMove(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                direction = new Vector3(value.ReadValue<Vector2>().x, 0, 0);
            }
            else
            {
                direction = Vector3.zero;
            }
        }
        #endregion
    }
}