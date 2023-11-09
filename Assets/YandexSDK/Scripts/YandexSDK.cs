using Game_;
using System;
using System.IO;
using System.Runtime.InteropServices;
using UI.Windows;
using UnityEngine;

namespace Yandex
{
    public class YandexSDK : MonoBehaviour
    {
        public static event Action<string> OnLanguageChanged;


        [DllImport("__Internal")]
        private static extern void ShowFullscreenAdvExtern();
        [DllImport("__Internal")]
        private static extern void ShowRewardedAdvExtern();
        [DllImport("__Internal")]
        private static extern void RateGameExtern();
        [DllImport("__Internal")]
        private static extern void UpdateLeaderboardExtern(int value);
        [DllImport("__Internal")]
        private static extern void ShowRewardToMoneyAdvExtern();
        [DllImport("__Internal")]
        private static extern void ShowRewardToContinueAdvExtern();
        public string lanugage
        {
            set
            {
                switch (value)
                {
                    case "ru":
                        break;
                    case "en":
                        break;
                    case "tr":
                        break;
                    default:
                        value = "en";
                        break;
                }
                OnLanguageChanged?.Invoke(value);
            }
        }

        private bool actualFlag;

        public void Reset()
        {
#if UNITY_EDITOR
            this.actualFlag = false;
#else 
            this.actualFlag = true;
#endif
        }


/*        public void SaveData(Data data)
        {
            if (this.actualFlag)
            {
                string json = JsonUtility.ToJson(data);
                SaveDataExtern(json);
            }
            else 
            {
                string json = JsonUtility.ToJson(data);
                File.WriteAllText(Application.streamingAssetsPath + "save.json", json);
                Debug.Log(Application.streamingAssetsPath);
            } Debug.Log("Save");
        }*/
/*        public void LoadData() 
        {
            if (this.actualFlag) 
            {
               // LoadDataExtern();
                this.lanugage = this.GetLanguage();
            } 
            else
            {
                this.lanugage = "ru";
                this.data = new Data();
            }
            Debug.Log($"I'm here done");
        }*/

/*        public void SetData(string value)
        {
            Debug.Log(value);
            var data = JsonUtility.FromJson<Data>(value);
            this.data = data;
            Debug.Log($"{this.gameObject.name}: Data loaded.");
        }*/

/*        public string GetLanguage()
        {
            if (this.actualFlag) return GetLanguageExtern();
            else return "en";
        }*/
        public void UpdateLeaderboard(int value)
        {
            if (this.actualFlag) UpdateLeaderboardExtern(value);
        }
        public void ShowFullscreenAdv()
        {
            if (this.actualFlag) 
            {
                ShowFullscreenAdvExtern();
                GameManager.GetController<EnvironmentController>().SetMuteState(false, true);
            }
            else
            {
                GameManager.GetController<EnvironmentController>().ResetMuteState();
            }
            Debug.Log("Fullscreen adv");
        }

        public void OnWatchFullscreenAdv()
        {
            GameManager.GetController<EnvironmentController>().ResetMuteState();
        }

        public void ShowRewardedAdv()
        {
            if (this.actualFlag) ShowRewardedAdvExtern();
        }

        public void RateGame()
        {
            if (this.actualFlag) 
            {
                GameManager.GetController<EnvironmentController>().SetMuteState(false, true);
                RateGameExtern();
            } 
            else
            {
                this.OnRateGame();
            }
        }
        public void OnRateGame()
        {
            GameManager.GetController<PlayerController>().RateGame();
            GameManager.GetController<UIController>().OpenWindow<UIStartWindow>();
            GameManager.GetController<EnvironmentController>().ResetMuteState();
        }
        public void ShowRewardToMoneyAdv()
        {
            if (this.actualFlag) 
            { 
                GameManager.GetController<EnvironmentController>().SetMuteState(false, true);
                ShowRewardToMoneyAdvExtern();
            } 
            else
            {
                this.OnWatchRewardToMoneyAdv();
            }
            Debug.Log("Reward adv");

        }

        public void OnWatchRewardToMoneyAdv()
        {
            GameManager.GetController<PlayerController>().AddMoney(100);
            GameManager.GetController<EnvironmentController>().ResetMuteState();
        }

        public void ShowRewardToContinueAdv()
        {
            if (this.actualFlag) 
            { 
                GameManager.GetController<EnvironmentController>().SetMuteState(false, true);
                ShowRewardToContinueAdvExtern();
            } 
            else
            {
                this.OnWatchRewardToContinueAdv();
            }
            Debug.Log("Continue adv");

        }

        public void OnWatchRewardToContinueAdv()
        {
            var gameplay = GameManager.GetController<GameplayController>();
            gameplay.Continue();
            GameManager.GetController<EnvironmentController>().ResetMuteState();
        }

            
        public void OnClose() => GameManager.GetController<EnvironmentController>().ResetMuteState();
     
    }
}
