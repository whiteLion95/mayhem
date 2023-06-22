using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class ParticlesManager : Singleton<ParticlesManager>
{
    [SerializeField]
    private ParticlesData _particlesData;
    [SerializeField]
    private List<ParticleSystem> _dirConfetti;
    [SerializeField]
    private ParticleSystem _showerConfetti;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;

        _gameManager.ProjectilesManager.OnProjCollision += HandleProjCollision;
        _gameManager.ProjectilesManager.OnProjOnTarget += HandleProjOnTarget;
        _gameManager.OnWin += HandleWin;
    }

    private void OnEnable()
    {
        Gun.OnFire += HandleGunFire;
        Enemy.OnHeadShot += HandleEnemyHeadShot;
    }

    private void OnDisable()
    {
        Gun.OnFire -= HandleGunFire;
        Enemy.OnHeadShot -= HandleEnemyHeadShot;
    }

    #region Methods
    public ParticleSystem PlayParticleOnce(Particles particleName, Vector3 position, Quaternion rotation)
    {
        ParticleData particleData = _particlesData[particleName];

        if (particleData == null || particleData.Prefab == null)
            return null;

        ParticleSystem playingParticle = LeanPool.Spawn(particleData.Prefab, position + particleData.Offset, rotation).GetComponentInChildren<ParticleSystem>();

        if (playingParticle == null)
            return null;

        playingParticle.transform.localScale = particleData.Scale;
        playingParticle.Play();
        StartCoroutine(DespawnParticle(playingParticle));

        return playingParticle;
    }

    public ParticleSystem PlayParticleOnce(Particles particleName, Vector3 position, Quaternion rotation, Color color)
    {
        ParticleSystem playingParticle = PlayParticleOnce(particleName, position, rotation);

        if (playingParticle == null)
            return null;

        ParticleSystem.MainModule settings = playingParticle.main;
        settings.startColor = new ParticleSystem.MinMaxGradient(color);

        return playingParticle;
    }

    public ParticleSystem PlayParticle(Particles particleName, Vector3 position, Quaternion rotation)
    {
        ParticleData particleData = _particlesData[particleName];

        if (particleData == null || particleData.Prefab == null)
            return null;

        ParticleSystem playingParticle = LeanPool.Spawn(particleData.Prefab, position + particleData.Offset, rotation).GetComponentInChildren<ParticleSystem>();

        if (playingParticle == null)
            return null;

        playingParticle.transform.parent.localScale = particleData.Scale;
        playingParticle.Play();

        return playingParticle;
    }

    private IEnumerator DespawnParticle(ParticleSystem particle)
    {
        if (particle)
        {
            yield return new WaitForSeconds(particle.main.duration);
            LeanPool.Despawn(particle);
        }
    }
    #endregion

    #region Handlers
    private void HandleGunFire(Gun gun, Projectile proj)
    {
        PlayParticleOnce(Particles.Muzzle, gun.FirePoint.position, Quaternion.LookRotation(gun.FirePoint.forward));

        if (gun.GetComponent<Player>())
            PlayParticleOnce(Particles.PlayerShells, gun.FirePoint.position, Quaternion.LookRotation(gun.FirePoint.forward));
    }

    private void HandleProjCollision(Projectile proj, Collision col)
    {
        if (proj.MyGun)
        {
            if (!proj.MyGun.GetComponent<Player>())
            {
                PlayParticleOnce(Particles.EnemyBulletHit, col.contacts[0].point, Quaternion.identity);

                if (col.gameObject.layer != LayerMask.NameToLayer("Player"))
                {
                    PlayParticleOnce(Particles.EnemyImpact, col.contacts[0].point, Quaternion.LookRotation(col.contacts[0].normal));
                    PlayParticleOnce(Particles.EnemyDebris, col.contacts[0].point, Quaternion.LookRotation(col.contacts[0].normal));
                }
            }
        }
    }

    private void HandleEnemyHeadShot(Enemy enemy, Vector3 pos)
    {
        PlayParticleOnce(Particles.HeadShot, pos, Quaternion.identity);
    }

    private void HandleProjOnTarget(Projectile proj, RaycastHit hitInfo)
    {
        Collider hitCollider = hitInfo.collider;

        if (hitCollider.gameObject.layer != LayerMask.NameToLayer("DestructionWall"))
        {
            PlayParticleOnce(Particles.PlayerBulletHit, proj.transform.position, Quaternion.identity);

            if (hitCollider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            {
                PlayParticleOnce(Particles.PlayerImpact, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                PlayParticleOnce(Particles.PlayerDebris, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            }
        }
    }

    private void HandleWin()
    {
        StartCoroutine(ConfettiRoutine());
    }

    private IEnumerator ConfettiRoutine()
    {
        foreach (var dirCon in _dirConfetti)
        {
            dirCon.Play();
        }

        yield return new WaitForSeconds(_dirConfetti[0].main.duration);

        _showerConfetti.Play();
    }
    #endregion
}