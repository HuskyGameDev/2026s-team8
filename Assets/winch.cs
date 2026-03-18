
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class winch : MonoBehaviour

{
   public LayerMask carLayer;
   public GameObject winchObject;
   public Vector3 winchPos;
   public Camera cam;
   public GameObject car;
   public Boolean attached = false;
   public SpringJoint joint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
        Vector3 screenPos = Input.mousePosition;
       // Vector3 mousePos = Camera.main.ScreenToWorldPoint(screenPos);
        Ray ray = cam.ScreenPointToRay(screenPos);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 2f);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100f, carLayer))
            {
                // hit.transform is reference to car
               winchPos = hit.transform.InverseTransformPoint(hit.point); // go from world space to car space
               attached = true;
            }
        }
        if(attached) {
        Vector3 worldWinch = car.transform.TransformPoint(winchPos);
        winchObject.transform.position = worldWinch;
        }


    }
}
