using MrSanmi.DungeonCrawler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MrSanmi.DungeonCrawler
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] protected List<Agent> _enemiesOfThisPool;
        [SerializeField] protected int _enemiesLeft;

        private void Start()
        {
            _enemiesLeft = _enemiesOfThisPool.Count;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (_enemiesLeft <= 0)
                {
                    print("I always come back");
                    for (int i = 0; i < _enemiesOfThisPool.Count; ++i)
                    {
                        if (_enemiesOfThisPool[i].GetComponentInChildren<HurtBox>()._currentHealthPoints <= 0)
                        {
                            _enemiesOfThisPool[i].GetComponentInChildren<HurtBox>()._currentHealthPoints = _enemiesOfThisPool[i].maxHealthPoints;
                            _enemiesOfThisPool[i].gameObject.SetActive(true);
                            _enemiesOfThisPool[i].GetComponent<Agent>().StateMechanic(StateMechanics.REVIVE);
                        }
                    }
                }
            }
        }

        #region PublicMethods

        public void SubstractEnemyFromEnemyCount()
        {
            _enemiesLeft -= 1;
        }

        #endregion
    }
}

