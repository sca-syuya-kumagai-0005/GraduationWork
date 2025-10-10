using UnityEngine;

public class MapMobilityManager : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    [SerializeField] float accelSpeed;
    [SerializeField] Vector3 addPosition;

    [SerializeField, Range(-1, 1)] int horizontal;
    [SerializeField,Range(-1, 1)] int vertical;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

      

        this.transform.position +=DirctionSet()*Time.deltaTime*speed;
    }

    Vector3 DirctionSet()
    {
        if (Input.GetKeyDown(KeyCode.W)) vertical = -1;
        if (Input.GetKeyDown(KeyCode.S)) vertical = 1;

        if (Input.GetKeyDown(KeyCode.A)) horizontal = 1;
        if (Input.GetKeyDown(KeyCode.D)) horizontal = -1;

        if (Input.GetKeyUp(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.S)) vertical = 1;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.W)) vertical = -1;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.D)) horizontal = -1;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.A)) horizontal = 1;
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            vertical = 0;
        }

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            horizontal = 0;
        }
        return new Vector3 (horizontal, vertical, 0);
    }

}
