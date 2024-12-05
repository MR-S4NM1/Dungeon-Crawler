using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrSanmi.DungeonCrawler
{
    [RequireComponent(typeof(Collider2D))]
    public class HurtBox : MonoBehaviour
    {
        #region Knobs

        //TODO: Stored in a Scriptable Object for robustness
        public float cooldownTime = 1.0f; //Damage (Hit Box) Per Second (Cooldown)

        #endregion

        #region References

        [SerializeField] protected Agent _agent;

        //[SerializeField] protected EnemyNPC _enemyNPC;
        //[SerializeField] protected PlayersAvatar _playersAvatar;

        #endregion

        #region RuntimeVariables

        protected bool _isInCooldown;
        [SerializeField] public short _currentHealthPoints;

        #endregion

        #region UnityMethods

        //private void OnDrawGizmos()
        //{
        //    #if UNITY_EDITOR
        //    if(_agent == null)
        //    {
        //        _agent = transform.parent.gameObject.GetComponent<Agent>();
        //    }
        //    #endif
        //}

        private void Start()
        {
            if (_agent == null)
            {
                _agent = transform.parent.gameObject.GetComponent<Agent>();
            }

            _currentHealthPoints = _agent.maxHealthPoints;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!_isInCooldown && _currentHealthPoints > 0) //To be immune or not
            {
                //I have the potential to be hurt by a HitBox :O
                if (other.gameObject.CompareTag("HitBox"))
                {
                    //This Hit Box may hurt me if it´s from another type
                    //of entity -> Friendly Fire = false.
                    if(other.gameObject.layer != gameObject.layer)
                    {
                        //Damn, I am about to be hurt DX
                        _currentHealthPoints -= 1; //other.gameObject.GetComponent<HitBox>().GetDamage;

                        switch (_agent)
                        {
                            case PlayersAvatar:
                                ((PlayersAvatar)_agent)?.UpdateUIHealth();
                                ((PlayersAvatar)_agent)?.ReturnOrbeToTheChestPublic();
                                ((PlayersAvatar)_agent)?.DamageAgent();
                                break;
                            case EnemyNPC:
                                //Debug.Break();
                                //((EnemyNPC)_agent)?.ApplyForce((transform.parent.gameObject.transform.position - other.gameObject.transform.position).normalized);
                                StartCoroutine(((EnemyNPC)_agent)?.DamageCorroutineEnemy());
                                break;
                        }


                        //Check if I am already dead
                        if(_currentHealthPoints <= 0)
                        {
                            //I will die, it´s time ;-;
                            _agent.StateMechanic(StateMechanics.DIE);

                            //TODO: Complete the administration of this state
                            //Animator
                            //Initialize, Executing and Finalize
                        }
                        else
                        {
                            StartCoroutine(Cooldown());
                        }
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isInCooldown) //To be immune or not
            {
                //I have the potential to be hurt by a HitBox :O
                if (other.gameObject.CompareTag("Bullet"))
                {
                    //This Hit Box may hurt me if it´s from another type
                    //of entity -> Friendly Fire = false.
                    if (other.gameObject.layer != gameObject.layer)
                    {
                        //Damn, I am about to be hurt DX
                        _currentHealthPoints -= 1; //other.gameObject.GetComponent<HitBox>().GetDamage;

                        switch (_agent)
                        {
                            case PlayersAvatar:
                                ((PlayersAvatar)_agent)?.UpdateUIHealth();
                                ((PlayersAvatar)_agent)?.ReturnOrbeToTheChestPublic();
                                ((PlayersAvatar)_agent)?.DamageAgent();
                                break;
                        }


                        //Check if I am already dead
                        if (_currentHealthPoints <= 1)
                        {
                            _agent.StateMechanic(StateMechanics.DIE);
                        }
                        else
                        {
                            StartCoroutine(Cooldown());
                        }
                    }
                }
            }
        }

        #endregion

        #region Coroutines

        protected IEnumerator Cooldown()
        {
            _isInCooldown = true; // To be immune for a certain time
            yield return new WaitForSeconds(cooldownTime);
            _isInCooldown = false;
        }

        #endregion

        #region GettersSetters

        #endregion
    }
}