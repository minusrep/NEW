using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Windows
{
    public class UIWindow : MonoBehaviour
    {
        public bool isActive
        {
            get => this.content.activeSelf;
            set
            {
                if (value) this.OnActive();
                this.content.SetActive(value);
            }
        }


        [SerializeField] private GameObject content;

        private void OnEnable()
        {
            GameManager.OnControllersCreatedEvent += this.OnControllersCreated;
        }

        private void OnDisable()
        {
            GameManager.OnControllersCreatedEvent -= this.OnControllersCreated;
        }

        private void Start()
        {

        }

        protected virtual void OnControllersCreated()
        {
            
        }

        protected virtual void OnActive()
        {

        }
    }

}
