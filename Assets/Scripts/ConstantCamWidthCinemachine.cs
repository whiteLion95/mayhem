using Cinemachine;
using UnityEngine;

public class ConstantCamWidthCinemachine : MonoBehaviour
{
    public Vector2 DefaultResolution = new Vector2(720, 1280);
    public bool useUpdate = false;

    private CinemachineVirtualCamera _virtCam;
    private float initialSize;
    private float targetAspect;
    private float initialFov;
    private float verticalFov;

    private void Awake()
    {
        _virtCam = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        Init();
        SetCamWidth();
    }

    private void Update()
    {
        if (useUpdate)
        {
            SetCamWidth();
        }
    }

    private void Init()
    {
        initialSize = _virtCam.m_Lens.OrthographicSize;
        targetAspect = DefaultResolution.x / DefaultResolution.y;
        initialFov = _virtCam.m_Lens.FieldOfView;
        verticalFov = CalcVerticalFov(initialFov, 1 / targetAspect);
    }

    private float CalcVerticalFov(float vFovInDeg, float aspectRatio)
    {
        float hFovInRads = vFovInDeg * Mathf.Deg2Rad;

        float vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio);

        return vFovInRads * Mathf.Rad2Deg;
    }

    private void SetCamWidth()
    {
        if (_virtCam.m_Lens.Orthographic)
        {
            float constantWidthSize = initialSize * (targetAspect / _virtCam.m_Lens.Aspect);
            _virtCam.m_Lens.OrthographicSize = constantWidthSize;
        }
        else
        {
            float constantWidthFov = CalcVerticalFov(verticalFov, _virtCam.m_Lens.Aspect);
            _virtCam.m_Lens.FieldOfView = constantWidthFov;
        }
    }
}
