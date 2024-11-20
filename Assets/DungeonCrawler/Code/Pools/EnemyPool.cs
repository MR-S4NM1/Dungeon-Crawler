using MrSanmi.DungeonCrawler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrSanmi.DungeonCrawler
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] protected int _maxEnemyHealthPoints;
        [SerializeField] protected List<EnemyNPC> _enemiesOfThisPool;
        [SerializeField] protected int _enemiesLeft;

        private void Start()
        {
            _enemiesLeft = _enemiesOfThisPool.Count;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(_enemiesLeft <= 0)
            {
                foreach (EnemyNPC enemy in _enemiesOfThisPool)
                {
                    if (enemy.GetComponent<HurtBox>().CurrentHealthPoints <= 0)
                    {
                        enemy.gameObject.SetActive(true);
                        enemy.GetComponent<HurtBox>().CurrentHealthPoints = _maxEnemyHealthPoints;
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

