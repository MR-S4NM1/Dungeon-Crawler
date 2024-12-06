using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrSanmi.DungeonCrawler
{
    public class BulletCode : MonoBehaviour
    {
        #region Knobs

        [SerializeField] public float bulletSpeed;

        #endregion

        #region References

        [SerializeField] protected Rigidbody2D _rb2D;

        #endregion


        #region RuntimeVariables

        [SerializeField] protected Vector2 bulletDirection;
        [SerializeField] protected bool _hasTouchedThePlayer;
        [SerializeField] public Transform _target;

        #endregion


        #region UnityMethods

        private void Start()
        {
            if(_rb2D == null)
            {
                _rb2D = GetComponent<Rigidbody2D>();
            }  
        }

        private void OnEnable()
        {
            StartCoroutine(StopPersecution());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void FixedUpdate()
        {
            //_rb2D.velocity = bulletDirection * bulletSpeed;

            bulletDirection = (_target.position - this.gameObject.transform.position).normalized;

            _rb2D.velocity = bulletDirection * bulletSpeed;
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
                    this.gameObject.transform.position = this.gameObject.transform.parent.gameObject.transform.position;
                }
            }
        }

        #endregion

        #region Corroutines

        protected IEnumerator StopPersecution()
        {
            yield return new WaitForSeconds(3.0f);

            this.gameObject.SetActive(false);
            this.gameObject.transform.position = this.gameObject.transform.parent.gameObject.transform.position;
        }

        #endregion

        #region GettersSetters

        public bool GetHasTouchedThePlayer
        {
            get { return _hasTouchedThePlayer;}
        }

        #endregion
    }

}
