
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class winch : MonoBehaviour

{
   public LayerMask carLayer;
   public GameObject winchObject;
   public GameObject winchPull;
   public Vector3 winchPos;
   public Vector3 pullWinchPos;
   public Camera cam;
   public GameObject car;
   public Boolean attached = false;
   public Boolean pull = false;
   private LineRenderer lineRenderer;
   public GameObject player;
   public SpringJoint joint;
   public float minWinchDistance = 4;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         // Add a LineRenderer component
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        // Set the material
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // Set the color
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.green;

        // Set the width
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;

        // Set the number of vertices
        lineRenderer.positionCount = 2;
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
                float winchDistance = Vector3.Distance(player.transform.position, hit.point);
                if(minWinchDistance > winchDistance) {
                // hit.transform is reference to car
               winchPos = hit.transform.InverseTransformPoint(hit.point); // go from world space to car space
               attached = true;
            }
        }
    }
        
        else if(Input.GetMouseButtonDown(1) && attached == true)
        {
            Vector3 screenPos = Input.mousePosition;
       // Vector3 mousePos = Camera.main.ScreenToWorldPoint(screenPos);
        Ray ray = cam.ScreenPointToRay(screenPos);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 2f);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100f))
            {
                float winchDistance = Vector3.Distance(player.transform.position, hit.point);
                if(minWinchDistance > winchDistance) {
                // hit.transform is reference to car
               pullWinchPos = hit.point; // gets pull winch position
               winchPull.transform.position = pullWinchPos; // set the pull winch position
               joint.maxDistance = Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
               pull = true;
            }
        }
        }
        if(attached) {
        Vector3 worldWinch = car.transform.TransformPoint(winchPos);
        winchObject.transform.position = worldWinch;
        }

        lineRenderer.SetPosition(0, winchObject.transform.position);
        if(pull)
        {
            //winchPull.transform.position = pullWinchPos; //set the second winches position
            lineRenderer.SetPosition(1, pullWinchPos);
            
           joint.maxDistance -= (float).5*Time.deltaTime;
        }
        else {
        lineRenderer.SetPosition(1, player.transform.position);
        }

    }
}
