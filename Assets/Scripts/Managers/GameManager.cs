using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Transform _finishPlace;

    private Player _player;

    public Action OnLose;
    public Action OnWin;
    public Action OnAllStagesComplete;

    public CoversManager CoversManager { get; private set; }
    public EnemiesManager EnemiesManager { get; private set; }
    public ProjectilesManager ProjectilesManager { get; private set; }
    public Transform FinishPlace => _finishPlace;

    protected override void Awake()
    {
        base.Awake();

        CoversManager = GetComponentInChildren<CoversManager>();
        EnemiesManager = GetComponentInChildren<EnemiesManager>();
        ProjectilesManager = GetComponentInChildren<ProjectilesManager>();
    }

    private void Start()
    {
        _player = Player.Instance;
    }

    public void Init()
    {
        _player.Init();

        CoversManager.Init();
        EnemiesManager.Init();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
