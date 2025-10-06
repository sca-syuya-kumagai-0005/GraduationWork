using UnityEditor.UIElements;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int health;
    private const int maxHealth = 100;
    public int Health { get { return health; }}
    public void formatting()
    {
        health = maxHealth;
    }

    public void fluctuationHealth(int value)
    {
        health += value;
    }
}
