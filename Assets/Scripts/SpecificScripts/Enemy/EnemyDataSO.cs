using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Objects/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    [SerializeField] private int maxLife = 3;
    [SerializeField] private int damage = 1;

    public int MaxLife => maxLife;
    public int Damage => damage;
}
