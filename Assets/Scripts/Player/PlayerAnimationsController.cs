using UnityEngine;

public class PlayerAnimationsController : MonoBehaviour
{
    [SerializeField] private string _idleTrigger = "idle";
    [SerializeField] private string _runTrigger = "run";
    [SerializeField] private string _battleIdleTrigger = "battleIdle";
    [SerializeField] private string _ourOfCoverTrigger = "outOfCover";
    [SerializeField] private string _fireTrigger = "fire";
    [SerializeField] private string _deadTrigger = "die";
    [SerializeField] private string _victoryTrigger = "victory";
    [SerializeField] private int _aimLayer = 1;
    [SerializeField] private float _aimLayerSpeed = 20f;

    private Player _player;
    private Animator _anim;
    private PlayerInBattleBehaviour _battling;
    private float _aimLayerWeight;
    private GameManager _gameManager;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    public void Init()
    {
        _player = Player.Instance;
        _battling = GetComponent<PlayerInBattleBehaviour>();
        _gameManager = GameManager.Instance;

        _player.OnNewState += HandlePlayerNewState;
        _battling.OnLookingOutFromCover += HandleLookingFromCover;
        _battling.OnGoingBackToCover += HandleGoingBackToCover;
        _player.FireController.OnFire += HandlerPlayerFire;
        _gameManager.OnWin += HandleWin;
    }

    private void Update()
    {
        _anim.SetLayerWeight(_aimLayer, Mathf.Lerp(_anim.GetLayerWeight(_aimLayer), _aimLayerWeight, Time.deltaTime * _aimLayerSpeed));
    }

    private void HandlePlayerNewState(Player.State newState)
    {
        switch (newState)
        {
            case Player.State.Default:
                _anim.SetTrigger(_idleTrigger);
                break;
            case Player.State.Running:
            case Player.State.RunningToFinish:
                _anim.ResetTrigger(_battleIdleTrigger);
                _anim.ResetTrigger(_fireTrigger);
                _anim.SetTrigger(_runTrigger);
                break;
            case Player.State.Battle:
                _anim.SetTrigger(_battleIdleTrigger);
                DoRunAimAnimation(false);
                break;
            case Player.State.Dead:
                _anim.SetTrigger(_deadTrigger);
                break;
        }
    }

    public void DoRunAimAnimation(bool toDo)
    {
        if (toDo)
            _aimLayerWeight = 1f;
        else
            _aimLayerWeight = 0f;
    }

    private void HandleLookingFromCover()
    {
        _anim.ResetTrigger(_battleIdleTrigger);
        _anim.SetTrigger(_ourOfCoverTrigger);
    }

    private void HandleGoingBackToCover()
    {
        _anim.ResetTrigger(_fireTrigger);
        _anim.ResetTrigger(_ourOfCoverTrigger);
        _anim.SetTrigger(_battleIdleTrigger);
    }

    private void HandlerPlayerFire()
    {
        _anim.ResetTrigger(_fireTrigger);
        _anim.SetTrigger(_fireTrigger);
    }

    private void HandleWin()
    {
        _anim.SetTrigger(_victoryTrigger);
    }
}
