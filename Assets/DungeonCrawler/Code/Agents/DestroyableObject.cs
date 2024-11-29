using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrSanmi.DungeonCrawler
{
    #region Enum

    public enum DestroyableObjectType
    {
        VESSEL,
        BUSH
    }

    #endregion

    #region Structs
    #endregion
    public class DestroyableObject : Agent
    {
        #region Knobs

        public DestroyableObjectType destroyableObjecttype;

        #endregion

        #region References

        [SerializeField] protected GameObject spawnedHealth;

        #endregion

        #region RunTimeVariables

        int percentageForSpawningObject;

        #endregion

        #region LocalMethods
        #endregion

        #region UnityMethods
        void Start()
        {

        }
        #endregion

        #region PublicMethods

        public void InstantiateObject()
        {
            print(gameObject.name + " InstantiateObject()" + " AHHHHHHHHHHHHHHHH");
            percentageForSpawningObject = UnityEngine.Random.Range(0, 10);

            switch (destroyableObjecttype)
            {
                case DestroyableObjectType.VESSEL:
                    Instantiate(spawnedHealth, this.gameObject.transform.position, Quaternion.identity);
                    break;
                case DestroyableObjectType.BUSH:
                    if (percentageForSpawningObject == 0)
                    {
                        Instantiate(spawnedHealth, this.gameObject.transform.position, Quaternion.identity);
                    }
                    break;
            }
        }

        #endregion

        #region GettersSetters
        #endregion
    }
}