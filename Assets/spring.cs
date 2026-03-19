using UnityEngine;

public class spring : MonoBehaviour
{
    public SpringJoint joint;
    public GameObject winch;
    public GameObject winchPull;
    public GameObject player;
    private LineRenderer lineRenderer;

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
        lineRenderer.SetPosition(0, winch.transform.position);
        lineRenderer.SetPosition(1, player.transform.position);

        if(Input.GetMouseButtonDown(1))
        {
            lineRenderer.SetPosition(1, winchPull.transform.position);
        }
        }
}
