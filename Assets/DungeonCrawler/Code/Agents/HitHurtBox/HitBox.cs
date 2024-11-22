using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrSanmi.DungeonCrawler
{
    [RequireComponent(typeof(Collider2D))]
    public class HitBox : MonoBehaviour
    {
        #region Knobs

        public int damage = 1;
        public float lifeTime = 1;

        #endregion

        #region RuntimeVariables

        protected bool _isHitBoxActivated;

        #endregion

        #region References

        [SerializeField] protected Collider2D _collider;

        #endregion

        #region UnityMethods

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            if (_collider == null)
            {
                _collider = GetComponent<Collider2D>();
            }
            #endif
        }

        #endregion

        #region PublicMethods

        public void ActivateHitBox()
        {
            if (_isHitBoxActivated)
            {
                StartCoroutine(Lifetime());
            }
        }

        #endregion

        #region Coroutines

        public IEnumerator Lifetime()
        {
            _isHitBoxActivated = true;
            _collider.enabled = true;
            yield return new WaitForSeconds(lifeTime);
            _collider.enabled = false;
            _isHitBoxActivated = false;
        }

        #endregion
    }

}