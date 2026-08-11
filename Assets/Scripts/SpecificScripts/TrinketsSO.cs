using UnityEngine;

[CreateAssetMenu(fileName = "shopMenu", menuName = "TrinketsSO/New Trinket", order = 1)]
public class TrinketsSO : ScriptableObject
{
    public string title;
    public string description;
    public int cost;
}
