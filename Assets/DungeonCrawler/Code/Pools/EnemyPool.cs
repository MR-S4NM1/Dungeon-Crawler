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
        [SerializeField] protected bool _firstTimeDetectingAPlayer;

        private void Start()
        {
            _firstTimeDetectingAPlayer = false;
            _enemiesLeft = 0;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (!_firstTimeDetectingAPlayer)
                {
                    for (int i = 0; i < _enemiesOfThisPool.Count; ++i)
                    {
                        _enemiesOfThisPool[i].gameObject.SetActive(true);
                    }
                    _firstTimeDetectingAPlayer = true;
                    _enemiesLeft = _enemiesOfThisPool.Count;
                }

                if ((_enemiesLeft <= 0) && _firstTimeDetectingAPlayer)
                {
                    for (int i = 0; i < _enemiesOfThisPool.Count; ++i)
                    {
                        if (_enemiesOfThisPool[i].GetComponentInChildren<HurtBox>()._currentHealthPoints <= 0)
                        {
                            _enemiesOfThisPool[i].GetComponentInChildren<HurtBox>()._currentHealthPoints = _enemiesOfThisPool[i].maxHealthPoints;
                            _enemiesOfThisPool[i].gameObject.SetActive(true);
                            _enemiesOfThisPool[i].GetComponent<Agent>().StateMechanic(StateMechanics.REVIVE);
                        }
                    }
                    _enemiesLeft = _enemiesOfThisPool.Count;
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

