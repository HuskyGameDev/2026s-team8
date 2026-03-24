
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
   private bool auto = false;
   public float minWinchDistance;
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

        lineRenderer.enabled = false;

        winchObject.SetActive(false);
        winchPull.SetActive(false);

        joint.anchor = joint.transform.InverseTransformPoint(winchPull.transform.position);

               joint.connectedAnchor = winchPos;

    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.G))
        {
            auto = !auto;
            print(auto);
        }

        if(Input.GetKeyDown(KeyCode.E)) // logic for picking up a winch on the ground
        {
                float dist = Vector3.Distance(player.transform.position, winchObject.transform.position);
                float distPull = Vector3.Distance(player.transform.position, winchPull.transform.position);
                if(dist < minWinchDistance)
            {
                attached = false;
                winchObject.SetActive(false);
                joint.maxDistance = float.PositiveInfinity;
                lineRenderer.enabled = false;
            }
                else if(distPull < minWinchDistance)
            {
                pull = false;
                winchPull.SetActive(false);
                joint.maxDistance = float.PositiveInfinity;
                lineRenderer.enabled = false;
                
            }
        }

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
                winchObject.SetActive(true);
                lineRenderer.enabled = true;
               winchPos = car.transform.InverseTransformPoint(hit.point);// go from world space to car space
               joint.maxDistance = Vector3.Distance(winchObject.transform.position, winchPull.transform.position);
               joint.anchor = joint.transform.InverseTransformPoint(winchPull.transform.position);

               joint.connectedAnchor = winchPos;
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
                if(minWinchDistance > winchDistance && hit.transform != player.transform) {
                // hit.transform is reference to car
                winchPull.SetActive(true);
                lineRenderer.enabled = true;
               pullWinchPos = hit.point; // gets pull winch position
               winchPull.transform.position = pullWinchPos; // set the pull winch position
               joint.maxDistance = Vector3.Distance(winchObject.transform.position, pullWinchPos);
                joint.anchor = joint.transform.InverseTransformPoint(winchPull.transform.position);

               joint.connectedAnchor = winchPos;

               pull = true;
            }
        }
        }
        if(attached) {
        Vector3 worldWinch = car.transform.TransformPoint(winchPos);
        winchObject.transform.position = worldWinch;
        if(!pull)
            {
                joint.maxDistance = 20;
                joint.anchor = joint.transform.InverseTransformPoint(player.transform.position);
            }
        }

        lineRenderer.SetPosition(0, winchObject.transform.position);
        if(pull)
        {
            //winchPull.transform.position = pullWinchPos; //set the second winches position
            lineRenderer.SetPosition(1, pullWinchPos);
            if(Input.GetKey(KeyCode.F)) {
           joint.maxDistance -= (float).5*Time.deltaTime;
            }
            else if(auto)
            {
                joint.maxDistance -= (float).5*Time.deltaTime;
            }
        }
        else {
        lineRenderer.SetPosition(1, player.transform.position);
        }

    }
}
