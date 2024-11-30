using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrSanmi.DungeonCrawler
{
    public enum EnvironmentStuffType
    {
        FLOOR_BUTTON,
        PEDESTAL,
        CHEST
    }

    public enum ButtonType
    {
        BRIDGE,
        PORTALS
    }

    public class InteractiveStuff : MonoBehaviour
    {
        #region Knobs

        public EnvironmentStuffType typeOfEnvironmentStuff;
        public ButtonType buttonType;

        #endregion

        #region References

        [SerializeField] protected GameReferee _gameReferee;
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] protected Sprite _interactedSprite;
        [SerializeField] protected Sprite _originalSprite;

        #endregion

        #region RuntimeVariables

        public bool pedestalBool;
        public bool chestBool;

        #endregion

        #region UnityMethods


        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (_gameReferee == null)
            {
                _gameReferee = GameObject.FindObjectOfType<GameReferee>();
            }
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
#endif

        }

        private void Start()
        {
            switch (typeOfEnvironmentStuff)
            {
                case EnvironmentStuffType.PEDESTAL:
                    pedestalBool = false;
                    break;
                case EnvironmentStuffType.CHEST:
                    chestBool = false;
                    break;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            ValidatePlayerTrigger(other);
        }

        #endregion

        protected void ValidatePlayerTrigger(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }
            switch (typeOfEnvironmentStuff)
            {
                case EnvironmentStuffType.FLOOR_BUTTON:
                    _spriteRenderer.sprite = _interactedSprite;
                    switch (buttonType)
                    {
                        case ButtonType.BRIDGE:
                            _gameReferee.DeactivatePathLocks("MiddlePathLocks");
                            break;
                        case ButtonType.PORTALS:
                            _gameReferee.ActivatePortalsOfTheGame();
                            break;
                    }
                    break;
            }
        }

        public void OpenChest()
        {
            if (typeOfEnvironmentStuff == EnvironmentStuffType.CHEST && !chestBool)
            {
                chestBool = true;
                _spriteRenderer.sprite = _interactedSprite;
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        public void CloseChest()
        {
            if (typeOfEnvironmentStuff == EnvironmentStuffType.CHEST)
            {
                chestBool = false;
                _spriteRenderer.sprite = _originalSprite;
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

    }
}