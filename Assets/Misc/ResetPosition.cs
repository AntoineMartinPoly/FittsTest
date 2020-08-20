using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResetPosition : MonoBehaviour
{
    
    public GameObject car;
    // public GameObject cam;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = car.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            car.transform.position = initialPosition;
            Debug.Log("reset car at : " + car.transform.position);
        }
    }
    
}
