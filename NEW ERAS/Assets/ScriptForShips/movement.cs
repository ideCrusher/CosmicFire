using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public static Vector3 a;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
              
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Po = new RaycastHit();
        if (Physics.Raycast(ray, out Po))
            a = new Vector3(Po.point.x, Po.point.y, Po.point.z);
    }
}
