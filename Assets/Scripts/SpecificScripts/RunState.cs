using UnityEngine;

public class RunState : IDontDestroy<RunState>
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private ShopManager sManager;
    [SerializeField] private WaveController wController;

    private void OnEnable()
    {
        stats = FindAnyObjectByType<PlayerStats>();
        sManager = FindAnyObjectByType<ShopManager>();
        wController = FindAnyObjectByType<WaveController>();
    }

    protected override void Awake()
    {
        GameManager.Instance?.checkpointSave.Restore(this); 
    }
    public RunMemento SaveState()
    {
        return new RunMemento(
            wController.waveCount, wController.Amount, sManager.Coins,
            stats.playerDamage, stats.playerHealth, stats.playerSpeed, stats.weaponCoolDown,
            stats.pricePlayerDamage, stats.pricePlayerHealth, stats.pricePlayerSpeed, stats.priceWeaponCoolDown);
    }

    public void RestoreState(RunMemento m)
    {
        wController.SetProgress(m.WaveCount, m.EnemysAmount);
        sManager.SetCoins(m.Coins);

        stats.playerDamage = m.PlayerDamage;
        stats.playerHealth = m.PlayerHealth;
        stats.playerSpeed = m.PlayerSpeed;
        stats.weaponCoolDown = m.WeaponCoolDown;

        stats.pricePlayerDamage = m.PriceplayerDamage;
        stats.pricePlayerHealth = m.PricePlayerHealth;
        stats.pricePlayerSpeed = m.PricePlayerSpeed;
        stats.priceWeaponCoolDown = m.PriceWeaponCoolDown;
    }
}
