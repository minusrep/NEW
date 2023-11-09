using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game_
{
    [RequireComponent(typeof(Animator))]
    public abstract class PickableObject : MonoBehaviour
    {
        public float moveSpeed => this._moveSpeed;
        [SerializeField] private float _moveSpeed;
        private Animator animator;

        private void Start()
        {
            this.animator = this.GetComponent<Animator>();
        }

        public void Pick()
        {
            this.animator.SetTrigger("Pick");
        }

        public void OnAnimationEnd()
        {
            this.gameObject.SetActive(true);
        }
    }

}
