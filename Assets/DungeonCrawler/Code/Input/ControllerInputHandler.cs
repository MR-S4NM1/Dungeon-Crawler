using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MrSanmi.DungeonCrawler
{
    #region Enum
    
    #endregion

    #region Structs
    #endregion

    public class ControllerInputHandler : MonoBehaviour
    {
        #region Knobs

        #endregion

        #region RunTimeVariables
        protected PlayerInput _playerInput;
        protected PlayersAvatar[] _allAvatarsInLevel;
        protected PlayersAvatar _avatarOfIndex;
        #endregion
        void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
            _allAvatarsInLevel = GameObject.FindObjectsOfType<PlayersAvatar>(true);
            foreach(PlayersAvatar avatar in _allAvatarsInLevel) 
            {
                if ((int)avatar.playerIndex == _playerInput.playerIndex)
                {
                    if (!avatar._hasAlreadyBeenActivated)
                    {
                        _avatarOfIndex = avatar;
                        _avatarOfIndex.gameObject.SetActive(true);
                        this.transform.parent = avatar.transform;
                        this.transform.localPosition = Vector2.zero;
                        avatar._hasAlreadyBeenActivated = true;
                    }
                }
            }
        }
        #region InputCallbackthMethods
        public void OnMove(InputAction.CallbackContext value)
        {
            _avatarOfIndex?.OnMOVE(value);
        }

        public void OnAttack(InputAction.CallbackContext value)
        {
            Debug.Log(gameObject.name + "Controller Input Handler - OnAttack()");
            _avatarOfIndex?.OnATTACK(value);
        }

        public void OnSprint(InputAction.CallbackContext value)
        {
            _avatarOfIndex?.OnSPRINT(value);
        }

        public void OnPause(InputAction.CallbackContext value)
        {
            _avatarOfIndex?.OnPAUSE(value);
        }

        public void OnInteract(InputAction.CallbackContext value)
        {
            _avatarOfIndex?.OnINTERACT(value);
        }

        public void ControllerVibration(float vibrationTime)
        {
            StartCoroutine(VibrationCorroutineForSeconds(vibrationTime));
        }

        #endregion

        #region Corroutines

        protected IEnumerator VibrationCorroutineForSeconds(float vibrationTime)
        {
            GetComponent<PlayerInput>().GetDevice<Gamepad>()?.SetMotorSpeeds(0.25f, 0.75f);

            yield return new WaitForSeconds(vibrationTime);

            GetComponent<PlayerInput>().GetDevice<Gamepad>()?.SetMotorSpeeds(0.0f, 0.0f);
        }

        #endregion
    }
}