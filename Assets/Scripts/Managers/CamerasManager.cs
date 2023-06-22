using UnityEngine;
using Utils;

public class CamerasManager : VirtualCamerasManager
{
    [SerializeField] private CinemachineCameraShake _inBattleShake;

    private Player _player;
    private GameManager _gameManager;

    public static new CamerasManager Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Instance = this;
    }

    private void Start()
    {
        _player = Player.Instance;
        _gameManager = GameManager.Instance;

        _player.OnNewState += HandlePlayerNewState;
        _player.HealthController.OnTakeDamage += HandlePlayerTookDamage;
        _gameManager.OnWin += HandleWin;
    }

    private void OnEnable()
    {
        BattleCamTrigger.OnPlayerTrigger += HandlePlayerBattleTrigger;
    }

    private void OnDisable()
    {
        BattleCamTrigger.OnPlayerTrigger -= HandlePlayerBattleTrigger;
    }

    private void HandlePlayerNewState(Player.State state)
    {
        if (state == Player.State.Running)
        {
            if (!_gameManager.CoversManager.CurCoverPlace.ShareCamWithPrevCoverPlace)
            {
                SwitchCamera("runCam");
            }
        }

        if (state == Player.State.RunningToFinish)
        {
            SwitchCamera("winCam");
        }
    }

    private void HandlePlayerBattleTrigger()
    {
        SwitchCamera("battleCam");
        SetTarget(_gameManager.CoversManager.CurCoverPlace.CamFocus);
    }

    private void HandlePlayerTookDamage(float damage)
    {
        if (damage > 0)
            _inBattleShake.ShakeSmooth();
    }

    private void HandleWin()
    {
        SwitchCamera("winCam");
    }
}
