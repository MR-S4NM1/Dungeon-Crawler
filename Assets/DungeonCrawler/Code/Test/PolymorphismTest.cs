using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MrSanmi.DungeonCrawler;

public class PolymorphismTest : MonoBehaviour
{
    #region Knobs

    // Polymorphism
    public Agent[] agentsInTheGame;

    #endregion

    #region RuntimeVariables

    int countAvatars;
    int countEnemies;
    int countDestroyableObjects;

    #endregion

    #region UnityMethods

    private void Start()
    {
        ManageAgents();
    }

    #endregion

    #region RuntimeMethods

    protected void ManageAgents()
    {
        foreach(Agent agent in agentsInTheGame)
        {

            // Casting example
            int randomNumber = (int)UnityEngine.Random.Range(0.0f, 100.0f);

            if(agent as EnemyNPC)
            {
                EnemyNPC enemy = (EnemyNPC)agent;
                countEnemies++;
                // TODO: invoke a method or manage an attribute of my enemy
            }
            else if(agent as PlayersAvatar)
            {
                PlayersAvatar avatar = (PlayersAvatar)agent;
                countAvatars++;
                // TODO: invoke a method or manage an attribute of my avatar
            }
            else if(agent as DestroyableObject)
            {
                DestroyableObject destroyableObject = (DestroyableObject)agent;
                countDestroyableObjects++;
                // TODO: invoke a method or manage an attribute of my destroyable oject
            }
        }

        print("Number of avatars " + countAvatars);
        print("Enemy count " + countEnemies);
        print("Number of destroyable objects " + countDestroyableObjects);
    }

    #endregion
}
