using Game_;
using System;
using System.Collections.Generic;

namespace Core
{
    public sealed class Game 
    {
        public bool isInitialized
        {
            get
            {
                var value = true;
                foreach (var controller in this.controllersMap)
                    if (!controller.Value.isInitialized) value = false;
                return value;
            }
        }
        
        private Dictionary<Type, Controller> controllersMap;


        public Game(Data data, string language = "en")
        {
            this.controllersMap = new Dictionary<Type, Controller>();
            var type = typeof(PlayerController);
            this.controllersMap[type] = new PlayerController(data);
            this.AddController<GameplayController>();
            this.AddController<EnvironmentController>();
            this.AddController<InputController>();
            this.AddController<UIController>();
            this.AddController<CaseController>();
        }

        public T GetController<T>() where T : Controller
        {
            this.controllersMap.TryGetValue(typeof(T), out var founded);
            return (T) founded;
        }

        private void AddController<T>() where T : Controller, new()
        {
            var type = typeof(T);
            this.controllersMap[type] = new T();
        }
    }
}