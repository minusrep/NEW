using Core;
using Game_.Environment;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace Game_
{
    public class EnvironmentController : Controller
    {
        public override bool isInitialized => this.model != null;

        public bool isCameraFocus { get => this.model.camera.isFocus; set => this.model.camera.isFocus = value; }
        public bool isMuted
        {
            get => this.model.audioSource.mute;
            private set
            {
                this.model.audioSource.mute = value;
            }
        }



        private EnvironmentModel model;
        private PlayerController player;
        private bool actualMuteState;
        public override void Initialize<T>(T model) 
        {
            this.model = model as EnvironmentModel;
            this.player = GameManager.GetController<PlayerController>();
            this.isMuted = this.player.muteState;
        }
        public void SetMuteState(bool value, bool isTemporally = false) 
        {
            this.isMuted = value;
            if (!isTemporally) this.player.muteState = value;
            this.InvokeMenuSound();
        }

        public void ResetMuteState()
        {
            this.isMuted = this.player.muteState;
        }


        public override void Update()
        {
        }

        public void FixedUpdate()
        {
            this.model.camera.ApplyFollowingTarget(this.player.position);

        }

        public void ResetCameraPosition() => this.model.camera.ResetPosition();
        public void InvokeSound(AudioClip clip) 
        {
            this.model.audioSource.pitch = Time.timeScale;
            this.model.audioSource.PlayOneShot(clip);
        }
        public void InvokeMenuSound() => this.model.audioSource.PlayOneShot(this.model.menuSound);
        public void StopSounds() => this.model.audioSource.Stop();


    }
}