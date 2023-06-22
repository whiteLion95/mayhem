using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "ScriptableObjects/GunData")]
public class GunDataSO : ScriptableObject
{
    [SerializeField] private float _firePower;
    [SerializeField] private int _bulletsCount = 4;
    [SerializeField] private float _reloadTime = 1f;
    [SerializeField] private int _bulletsPerReload = 1;
    [SerializeField] private float _bulletTweenSpeed = 5f;
    [SerializeField] private float _rapidFireDelay = 0.1f;

    public float FirePower { get => _firePower; }
    public int BulletsCount { get => _bulletsCount; }
    public float ReloadTime { get => _reloadTime; }
    public int BulletsPerReload { get => _bulletsPerReload; }
    public float BulletTweenSpeed { get => _bulletTweenSpeed; }
    public float RapidFireDelay { get => _rapidFireDelay; }
}