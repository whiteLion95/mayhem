using UnityEngine;

public class CoverPlace : MonoBehaviour
{
    [SerializeField]
    private float _lookOutXOffset = 0.5f;
    [SerializeField]
    private bool _shareCameraWithPrevCoverPlace;
    [SerializeField]
    private Transform _camFocus;

    public float LookOutXOffset => _lookOutXOffset;
    public bool ShareCamWithPrevCoverPlace => _shareCameraWithPrevCoverPlace;
    public Transform CamFocus => _camFocus;
}
