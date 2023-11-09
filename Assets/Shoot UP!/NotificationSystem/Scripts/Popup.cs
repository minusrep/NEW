using Game_;
using System.Collections;
using UnityEngine;

namespace Popups
{
    [RequireComponent(typeof(Animator))]
    public class Popup : MonoBehaviour
    {
        private Animator animator;

        private void Start()
        {
            this.animator = this.GetComponent<Animator>();
        }
        public virtual void Invoke()
        {
            this.gameObject.SetActive(true);
        }
        public virtual void Close()
        {
            this.animator.SetTrigger("Close");
            this.InvokeMenuSound();
        }

        public void OnAnimationEnd()
        {
            this.gameObject.SetActive(false);
        }

        protected void InvokeMenuSound() => GameManager.GetController<EnvironmentController>().InvokeMenuSound();
    }
}