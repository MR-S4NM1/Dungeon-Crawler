using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrSanmi.DungeonCrawler
{
    public enum portalTypes
    {
        COMMON, // Portals that you can acceess during the levels.
        FINAL // Portals that make you change from screen.
    }

    public class Portal : MonoBehaviour
    {
        #region knobs

        public portalTypes portalType;

        #endregion

        #region References

        [SerializeField] protected GameReferee _gameReferee;

        #endregion

        #region RuntimeVariables

        //We declare our Hash Set, which will contain our Player that will be transported.
        //We use a Hash Set, because we don´t want our players to be transported indefinitely, so this guarantees that our players only appear one time in the set.
        protected HashSet<GameObject> portalPlayer;

        //We declare and set the position where the Player will be teleported.
        [SerializeField] protected Transform portalPlayerDestination;

        #endregion

        #region UnityMethods

        private void Start()
        {
            if (portalType == portalTypes.COMMON)
            {
                portalPlayer = new HashSet<GameObject>(); //We initialize our Hash Set here since it's a runtime variables :P
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (portalType)
            {
                case portalTypes.COMMON:
                    ValidateTriggerWithPlayerWhenItIsANormalPortal(other);
                    break;
                case portalTypes.FINAL:
                    ValidateTriggerWithPlayerWhenItIsAFinalPortal(other);
                    break;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(portalType == portalTypes.COMMON)
            {
                ValidateExitTriggerWithPlayerWhenItIsANormalPortal(other);
            }
        }

        private void OnDrawGizmos()
        {
            if(_gameReferee == null)
            {
                _gameReferee = GameObject.FindFirstObjectByType<GameReferee>();
            }
        }

        #endregion

        #region RuntimeMethods

        protected void ValidateTriggerWithPlayerWhenItIsANormalPortal(Collider2D other)
        {
            //If we detect a Player, then we add it to the Hash Set, and then we transport it to the other location.
            //NOTE: IF IT ALREADY CONTAINS THE PLAYER, IT WON'T TRANSPORT IT.
            if (other.gameObject.CompareTag("Player"))
            {
                if (portalPlayer.Contains(other.gameObject))
                {
                    return;
                }

                if (portalPlayerDestination.TryGetComponent(out Portal destination))
                {
                    destination.portalPlayer.Add(other.gameObject);
                }

                other.transform.position = portalPlayerDestination.transform.position;
            }
        }

        protected void ValidateExitTriggerWithPlayerWhenItIsANormalPortal(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                portalPlayer.Remove(other.gameObject);
            }
        }

        protected void ValidateTriggerWithPlayerWhenItIsAFinalPortal(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _gameReferee.WinGame();
            }
        }

        #endregion


        #region GettersSetters

        #endregion
    }

}