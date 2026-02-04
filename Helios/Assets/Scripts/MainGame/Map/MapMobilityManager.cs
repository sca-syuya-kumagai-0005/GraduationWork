using UnityEngine;

public class MapMobilityManager : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    [SerializeField] float accelSpeed;
    [SerializeField] Vector3 addPosition;

    [SerializeField, Range(-1, 1)] int horizontal;
    [SerializeField, Range(-1, 1)] int vertical;

    string northKey = "W";
    public string NorthKey {  get { return northKey; } }    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {


        Vector3 mouseDelta = Input.mousePositionDelta;
        if (Input.GetMouseButton(1))
        {
            this.transform.position += mouseDelta * Time.deltaTime;
        }
        this.transform.position += DirctionSet() * Time.deltaTime / speed;
    }

    Vector3 DirctionSet()
    {
        switch (northKey)
        {
            case "U":
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
                    break;
                }
            case "D":
                {

                    if (Input.GetKeyDown(KeyCode.W)) vertical = 1;
                    if (Input.GetKeyDown(KeyCode.S)) vertical = -1;

                    if (Input.GetKeyDown(KeyCode.A)) horizontal = -1;
                    if (Input.GetKeyDown(KeyCode.D)) horizontal = 1;

                    if (Input.GetKeyUp(KeyCode.W))
                    {
                        if (Input.GetKey(KeyCode.S)) vertical = -1;
                    }

                    if (Input.GetKeyUp(KeyCode.S))
                    {
                        if (Input.GetKey(KeyCode.W)) vertical = 1;
                    }

                    if (Input.GetKeyUp(KeyCode.A))
                    {
                        if (Input.GetKey(KeyCode.D)) horizontal = 1;
                    }

                    if (Input.GetKeyUp(KeyCode.D))
                    {
                        if (Input.GetKey(KeyCode.A)) horizontal = -1;
                    }

                    if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
                    {
                        vertical = 0;
                    }

                    if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                    {
                        horizontal = 0;
                    }
                    break;
                }
            case "L":
                {

                    if (Input.GetKeyDown(KeyCode.W)) horizontal = 1;
                    if (Input.GetKeyDown(KeyCode.S)) horizontal = -1;

                    if (Input.GetKeyDown(KeyCode.A)) vertical = 1;
                    if (Input.GetKeyDown(KeyCode.D)) vertical = -1;

                    if (Input.GetKeyUp(KeyCode.W))
                    {
                        if (Input.GetKey(KeyCode.S)) horizontal = -1;
                    }

                    if (Input.GetKeyUp(KeyCode.S))
                    {
                        if (Input.GetKey(KeyCode.W)) horizontal = 1;
                    }

                    if (Input.GetKeyUp(KeyCode.A))
                    {
                        if (Input.GetKey(KeyCode.D)) vertical = -1;
                    }

                    if (Input.GetKeyUp(KeyCode.D))
                    {
                        if (Input.GetKey(KeyCode.A)) vertical = 1;
                    }

                    if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
                    {
                        horizontal = 0;
                    }

                    if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                    {
                        vertical = 0;
                    }
                    break;
                }
            case "R":
                {

                    if (Input.GetKeyDown(KeyCode.W)) horizontal =- 1;
                    if (Input.GetKeyDown(KeyCode.S)) horizontal = 1;

                    if (Input.GetKeyDown(KeyCode.A)) vertical = 1;
                    if (Input.GetKeyDown(KeyCode.D)) vertical = -1;

                    if (Input.GetKeyUp(KeyCode.W))
                    {
                        if (Input.GetKey(KeyCode.S)) horizontal = 1;
                    }

                    if (Input.GetKeyUp(KeyCode.S))
                    {
                        if (Input.GetKey(KeyCode.W)) horizontal = -1;
                    }

                    if (Input.GetKeyUp(KeyCode.A))
                    {
                        if (Input.GetKey(KeyCode.D)) vertical = 1;
                    }

                    if (Input.GetKeyUp(KeyCode.D))
                    {
                        if (Input.GetKey(KeyCode.A)) vertical = -1;
                    }

                    if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
                    {
                        horizontal = 0;
                    }

                    if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                    {
                        vertical = 0;
                    }
                    break;
                }
        }

        return new Vector3(horizontal, vertical, 0);
    }

}
