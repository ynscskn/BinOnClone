using UnityEngine;
using DG.Tweening;

public class M_Menu : MonoBehaviour
{
    public GameObject MainMenuPanelPrefab;
    public GameObject GamePanelPrefab;
    public GameObject PausePanelPrefab;
    public GameObject CompletePanelPrefab;

    [HideInInspector] public GameObject CurrentPanel;

    private void Awake()
    {
        M_Observer.OnGameCreate += GameCreate;
        M_Observer.OnGameReady += GameReady;
        M_Observer.OnGameStart += GameStart;
        M_Observer.OnGamePause += GamePause;
        M_Observer.OnGameFail += GameFail;
        M_Observer.OnGameComplete += GameComplete;
        M_Observer.OnGameRetry += GameRetry;
        M_Observer.OnGameContinue += GameContinue;
        M_Observer.OnGameNextLevel += GameNextLevel;
    }

    private void OnDestroy()
    {
        M_Observer.OnGameCreate -= GameCreate;
        M_Observer.OnGameReady -= GameReady;
        M_Observer.OnGameStart -= GameStart;
        M_Observer.OnGamePause -= GamePause;
        M_Observer.OnGameFail -= GameFail;
        M_Observer.OnGameComplete -= GameComplete;
        M_Observer.OnGameRetry -= GameRetry;
        M_Observer.OnGameContinue -= GameContinue;
        M_Observer.OnGameNextLevel -= GameNextLevel;
    }

    private void Start()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(MainMenuPanelPrefab, transform);
    }

    private void GameCreate()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(MainMenuPanelPrefab, transform);
    }

    private void GameReady()
    {

    }

    private void GameStart()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(GamePanelPrefab, transform);
    }

    private void GamePause()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(PausePanelPrefab, transform);
    }
    private void GameFail()
    {
        //print("GameFail");
    }

    private void GameComplete()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(CompletePanelPrefab, transform);
    }

    private void GameRetry()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(GamePanelPrefab, transform);
    }

    private void GameContinue()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(GamePanelPrefab, transform);
    }

    private void GameNextLevel()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(GamePanelPrefab, transform);
    }

    void DeleteCurrentPanel()
    {
        if (CurrentPanel != null)
        {
            Destroy(CurrentPanel);
            CurrentPanel = null;
        }
    }
}
