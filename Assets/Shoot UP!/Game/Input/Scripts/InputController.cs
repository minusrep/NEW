using Core;
using Game_.Input_;
using System.Collections;
using UI.Windows;
using UnityEngine;

namespace Game_
{
    public class InputController : Controller
    {
        public override bool isInitialized => this.model != null;

        private InputModel model;
        private PlayerController player;
        private GameplayController gameplay;
        private UIController ui;
        private EnvironmentController environment;
        private CaseController _case;
        public override void Initialize<T>(T model)
        {
            this.model = model as InputModel;
            this.player = GameManager.GetController<PlayerController>();
            this.gameplay = GameManager.GetController<GameplayController>();
            this.ui = GameManager.GetController<UIController>();
            this._case = GameManager.GetController<CaseController>();
            this.environment = GameManager.GetController<EnvironmentController>();
            GameManager.OnGameLoadedEvent += this.OnGameLoaded;
        }

        private void OnGameLoaded()
        {
            this.ui.OpenWindow<UIStartWindow>();
        }

        public override void Update()
        {
            base.Update();
        }

        public void Start()
        {
            this.ui.OpenWindow<UIHUD>();
            this.gameplay.OnStart();
        }

        public void Input() 
        {
            Debug.Log("Input");
            this.player.OnInput();
        }

        public void RollCase() 
        {
            this.environment.InvokeMenuSound();
            this._case.Invoke();
        }

        public void OpenRestartWindow() 
        {
            this.ui.OpenWindow<UIRestartWindow>();
            this.player.OnTryEnded();
        } 

        public void ChangeMuteState()
        {
            this.environment.SetMuteState(!this.environment.isMuted);
        }
    }
}