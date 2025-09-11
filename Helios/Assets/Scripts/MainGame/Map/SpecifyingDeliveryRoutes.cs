using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//Ç±ÇÍÇÕîzíBé“Ç…Ç¬ÇØÇÈscriptÇ≈Ç∑ÅB
public class SpecifyingDeliveryRoutes : MonoBehaviour
{
    [SerializeField]List<int> routes=new List<int>();
    [SerializeField]List<Vector3> routesPosition = new List<Vector3>();
    bool memorying = false;
    public bool Memorying { get { return memorying; } }
    [SerializeField]GameObject driver;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MemoryRoute(int objectPositionID,Vector2 position)
    {
        if(!memorying)return;
        routes.Add(objectPositionID);
        routesPosition.Add(position);
    }

    public void MemoryStart()
    {
        memorying = true;
    }

    public void MemoryEnd()
    {
        memorying = false;
        StartCoroutine(DriverMove());
    }

    private IEnumerator DriverMove()
    {
        for(int i=0;i<routesPosition.Count;i++)
        {
            Vector3 dir = (routesPosition[i] - driver.transform.position).normalized;
            float dist = Mathf.Abs(routesPosition[i].magnitude - driver.transform.position.magnitude);
            while (dist>0.01f)
            {
                Vector2 vec = driver.transform.position+dir*Time.deltaTime;
                dist = Mathf.Abs(routesPosition[i].magnitude - driver.transform.position.magnitude);
                driver.transform.position = vec;
                yield return null;
            }
            driver.transform.position = routesPosition[i];
            
            
            
        }
        yield return null;
    }

}
