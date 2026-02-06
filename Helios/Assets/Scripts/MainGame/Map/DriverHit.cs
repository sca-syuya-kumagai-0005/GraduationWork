using UnityEngine;

public class DriverHit : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] SpecifyingDeliveryRoutes sDR;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Beam"))
        {
            sDR.Breaking[id] = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Beam"))
        {
            sDR.Breaking[id] = true;
        }
    }
}
