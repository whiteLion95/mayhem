using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class SkinnedMeshCollider : MonoBehaviour
{
    [SerializeField]
    private bool _updateOnStart;

    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private MeshCollider _meshCol;

    private void Awake()
    {
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        _meshCol = GetComponent<MeshCollider>();
    }

    private void Start()
    {
        if (_updateOnStart)
        {
            UpdateCollider();
        }
    }

    public void UpdateCollider()
    {
        Mesh colliderMesh = new Mesh();
        _skinnedMeshRenderer.BakeMesh(colliderMesh);
        _meshCol.sharedMesh = null;
        _meshCol.sharedMesh = colliderMesh;
    }
}
