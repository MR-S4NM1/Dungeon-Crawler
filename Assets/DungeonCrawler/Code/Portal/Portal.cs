using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrSanmi.DungeonCrawler
{
    public class Portal : MonoBehaviour
    {
        #region knobs

        #endregion

        #region RuntimeVariables

        //We declare our Hash Set, which will contain our Player that will be transported.
        protected HashSet<GameObject> portalPlayer;

        //We declare and set the position where the Player will be teleported.
        [SerializeField] protected Transform portalPlayerDestination;

        #endregion

        private void Start()
        {
            portalPlayer = new HashSet<GameObject>(); //We initialize our Hash Set here since it's a runtime variables :P
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            ValidateTriggerWithPlayer(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            ValidateExitTriggerWithPlayer(other);
        }

        #region RuntimeMethods

        protected void ValidateTriggerWithPlayer(Collider2D other)
        {
            //If we detect a Player, then we add it to the Hash Set, and then we transport it to the other location.
            //NOTE: IF IT ALREADY CONTAINS THE PLAYER, IT WON'T TELEPORT IT.
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

        protected void ValidateExitTriggerWithPlayer(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                portalPlayer.Remove(other.gameObject);
            }
        }

        #endregion


        #region GettersSetters

        #endregion
    }

}