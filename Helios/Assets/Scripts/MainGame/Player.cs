using UnityEngine;

public class Player : MonoBehaviour
{
    private int health;
    public int Health { get { return health; } set { health = value; } }
    [SerializeField] private GameObject progressGraph;
    private int progress;
    private float progressLate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
