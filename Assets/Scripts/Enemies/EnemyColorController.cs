using Utils;

public class EnemyColorController : ColorsController
{
    private Enemy _enemy;

    protected override void Start()
    {
        base.Start();

        _enemy = GetComponent<Enemy>();

        _enemy.HealthController.OnTakeDamage += HandleEnemyTakeDamage;
    }

    private void HandleEnemyTakeDamage(float damage)
    {
        Blink();
    }
}
