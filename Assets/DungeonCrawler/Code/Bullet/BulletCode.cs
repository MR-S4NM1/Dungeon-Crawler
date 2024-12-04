using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrSanmi.DungeonCrawler
{
    public class BulletCode : MonoBehaviour
    {
        #region Knobs

        [SerializeField] public float bulletSpeed;
        [SerializeField] public float bulletLifetime;
        #endregion

        #region References

        [SerializeField] protected Rigidbody2D _rb2D;

        #endregion


        #region RuntimeVariables

        [SerializeField] public Vector2 bulletDirection;
        [SerializeField] protected bool _hasTouchedThePlayer;

        #endregion


        #region UnityMethods

        private void Start()
        {
            if(_rb2D == null)
            {
                _rb2D = GetComponent<Rigidbody2D>();
            }  
        }

        private void FixedUpdate()
        {
            //_rb2D.velocity = bulletDirection * bulletSpeed;

            _rb2D.MovePosition(bulletDirection * bulletSpeed);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            ValidateCollisionWithPlayer(other);
        }

        #endregion


        #region PublicMethods


        #endregion

        #region LocalMethods


        protected void ValidateCollisionWithPlayer(Collider2D other)
        {
            if (other.gameObject.CompareTag("HurtBox"))
            {
                if ((other.gameObject.layer != gameObject.layer) && (other.gameObject.transform.parent.gameObject.CompareTag("Player")))
                {
                    this.gameObject.SetActive(false);
                }
            }
        }

        #endregion

        #region Corroutines

        #endregion

        #region GettersSetters

        public bool GetHasTouchedThePlayer
        {
            get { return _hasTouchedThePlayer;}
        }

        #endregion
    }

}
