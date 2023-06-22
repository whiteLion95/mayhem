using Lean.Touch;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAimingBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform _aimTarget;
    [SerializeField]
    private LayerMask _layerMask;
    [SerializeField]
    private Rig _aimRig;
    [SerializeField]
    private float _aimSpeed = 20f;

    private Player _player;
    private bool _isAimed;
    private float _aimWeight;

    public bool IsAimed => _isAimed;

    void Start()
    {
        _player = Player.Instance;

        _player.OnNewState += HandlePlayerState;
    }

    private void Update()
    {
        if (!_player.IsDead)
        {
            _aimRig.weight = Mathf.Lerp(_aimRig.weight, _aimWeight, Time.deltaTime * _aimSpeed);
        }
    }

    public void Aim(LeanFinger finger)
    {
        PlaceAimTarget(finger);
        _aimWeight = 1f;
    }

    public void UnAim()
    {
        _aimWeight = 0f;
    }

    private void PlaceAimTarget(LeanFinger finger)
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(finger.ScreenPosition);

        if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, _layerMask))
        {
            _aimTarget.transform.position = hitInfo.point;
        }
    }

    private void HandlePlayerState(Player.State newState)
    {
        if (newState == Player.State.Battle)
        {
            if (_aimWeight > 0f)
                UnAim();
        }

        if (newState == Player.State.Dead)
            _aimRig.weight = 0f;
    }
}
