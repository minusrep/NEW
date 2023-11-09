using System.Collections;
using UnityEngine;
using Core;
using System;
using Yandex;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public sealed class GameManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SaveDataExtern(string data);
    [DllImport("__Internal")]
    private static extern void LoadDataExtern();
    [DllImport("__Internal")]
    private static extern string GetLanguageExtern();



    public static event Action OnControllersCreatedEvent;
    public static event Action OnGameLoadedEvent;

    public static YandexSDK yandexSDK => instance._yandexSDK;
    public static string language { get; private set; }
    private static GameManager instance;
    public static T GetController<T>() where T : Controller => instance.game.GetController<T>();
    public static void StartRoutine(IEnumerator routine) => instance.StartCoroutine(routine);
    private Game game;
    private YandexSDK _yandexSDK;
    private Data data;
    private bool actualFlag;
    private bool isYandexLoaded;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
#if UNITY_EDITOR
            this.isYandexLoaded = true;
            this.actualFlag = false;
#else 
            this.isYandexLoaded = false;
            this.actualFlag = true;
#endif
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start() 
    {
        this.Initialize();
    } 

    private void Initialize() => this.StartCoroutine(this.InitializeRoutine());
    private void InitializeYandexSDK()
    {
        this._yandexSDK = new GameObject("YandexSDK").AddComponent<YandexSDK>();
        this._yandexSDK.gameObject.transform.parent = this.gameObject.transform;
        this._yandexSDK.Reset();
    }
    private IEnumerator InitializeRoutine()
    {
        var loadingScreen = Instantiate(Resources.Load<GameObject>("[LOADING SCREEN]"));
        this.InitializeYandexSDK();
       // yield return new WaitUntil(() => this.isYandexLoaded);
        this.LoadData();
        yield return new WaitUntil(() => this.data != null);
        //this.game = new Game(this._yandexSDK.data);
        this.game = new Game(this.data);
        OnControllersCreatedEvent?.Invoke();
        if (SceneManager.GetActiveScene().name == "LoadingScene") SceneManager.LoadScene("GameScene");
        yield return new WaitUntil(() => this.game.isInitialized);
        language = GetLanguageExtern();
        Debug.Log("LANGuAGE: " + language);
        OnGameLoadedEvent?.Invoke();
        Debug.Log($"{this.gameObject.name}: Initialized.");
        loadingScreen.SetActive(false);
    }

    private void LoadData()
    {
#if UNITY_EDITOR
        this.data = new Data();
        language = "ru";

#else
        LoadDataExtern();
        //language = GetLanguageExtern();

#endif
        Debug.Log("LANGuAGE: " + language);

        Debug.Log("Data loaded manager");

    }
    public void SetData(string value)
    {
        var data = JsonUtility.FromJson<Data>(value);
        this.data = data;
        Debug.Log($"{this.gameObject.name}: Data loaded.");
    }

    public void SetLanguage(string value)
    {
        language = value;
        Debug.Log("Setten");
    }

    public static void SaveData(Data data)
    {
        if (instance.actualFlag)
        {
            string json = JsonUtility.ToJson(data);
            SaveDataExtern(json);
        }
        else
        {
            Debug.Log(Application.streamingAssetsPath);
        }
        Debug.Log("Save");

    }
}
