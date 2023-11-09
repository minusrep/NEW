using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Game_.Environment
{
    public class CameraBehavior : MonoBehaviour
    {
        public bool isFocus
        {
            get => this.processVolume.profile.GetSetting<LensDistortion>().scale == this.focusScale;
            set
            {
                var lensDistrotion = this.processVolume.profile.GetSetting<LensDistortion>();
                if (value) lensDistrotion.scale.Override(this.focusScale);
                else lensDistrotion.scale.Override(this.defaultScale);
                lensDistrotion.active = true;
            }
        }

        [SerializeField] private float smoothness;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Vector3 startPosition;
        [Space]
        [Header("PostProccesing Settings")]
        [SerializeField] private PostProcessVolume processVolume;
        [SerializeField] private float defaultScale;
        [SerializeField] private float focusScale;
        private void Start()
        {
            this.startPosition = this.transform.position;
            this.isFocus = false;
        }

        public void ApplyFollowingTarget(Vector3 position)
        {
            var toPosition = position;
            if (toPosition.y < this.transform.position.y) return;
            toPosition.y += this.offset.y;
            toPosition.z += this.offset.z;
            toPosition.x = 0f;
            this.transform.position = Vector3.Lerp(this.transform.position, toPosition, this.smoothness * Time.deltaTime);
        }

        public void ResetPosition()
        {
            this.transform.position = this.startPosition;
        }
    }

}
