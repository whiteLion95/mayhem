using UnityEngine;

public class PlayerHitPlace : MonoBehaviour
{
    [SerializeField]
    private float _yOffSet;
    [SerializeField]
    private float _inRunPredictionTime = 0.5f;

    private Player _player;
    private GameManager _gameManager;
    private PlayerRunningBehaviour _runningBehaviour;

    void Start()
    {
        _player = Player.Instance;
        _gameManager = GameManager.Instance;
        _runningBehaviour = _player.GetComponent<PlayerRunningBehaviour>();

        _player.OnNewState += HandlePlayerState;
    }

    private void HandlePlayerState(Player.State newState)
    {
        if (newState == Player.State.Battle)
        {
            CoverPlace curCovPlace = _gameManager.CoversManager.CurCoverPlace;
            transform.localPosition = new Vector3(curCovPlace.LookOutXOffset, _yOffSet, 0f);
        }

        if (newState == Player.State.Running)
            transform.localPosition = new Vector3(_runningBehaviour.MoveSpeed * _inRunPredictionTime, _yOffSet, 0f);
    }
}
