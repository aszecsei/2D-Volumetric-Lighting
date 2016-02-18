using UnityEngine;
using System.Collections;

public class LightShafter : MonoBehaviour {

    // The number of rays to draw
    [SerializeField]
    [Range(1, 720)]
    public int numberOfRays = 720;

    public float lightDistance = 5;

    [SerializeField]
    [Range(-360, 360)]
    public float lightAngle = -90;

    [SerializeField]
    [Range(0, 360)]
    public float lightCone = 90;

    [SerializeField]
    public float lightSourceSize = 0;

    public LayerMask layerMask;

    public bool DrawDebugRays = true;

    public Color lightColor = Color.white;

    private GameObject[] shafts;

    public Material shaftMaterial;

    public GameObject PointLight;

    [Range(0, 1)]
    public float shaftWidth = 0.2f;

    void Start()
    {
        Vector3 initialLocation = transform.position + (new Vector3(Mathf.Cos(Mathf.Deg2Rad * (lightAngle - 90)), Mathf.Sin(Mathf.Deg2Rad * (lightAngle - 90))) * lightSourceSize / 2);
        Vector3 locationOffset = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (lightAngle - 90)), Mathf.Sin(Mathf.Deg2Rad * (lightAngle - 90))) * lightSourceSize / numberOfRays;
        float RaySpacing = lightCone / numberOfRays;
        float initialAngle = lightAngle - (lightCone / 2);

        PointLight.GetComponent<Light>().color = lightColor;

        shafts = new GameObject[numberOfRays];
        for (int i = 0; i < numberOfRays; i++)
        {
            shafts[i] = new GameObject();
            shafts[i].name = "LightShaft " + (i + 1);
            shafts[i].transform.parent = transform;
            shafts[i].transform.position = transform.position;
            shafts[i].layer = LayerMask.NameToLayer("Light");
            LineRenderer shaftRenderer = shafts[i].AddComponent<LineRenderer>();
            shaftRenderer.useWorldSpace = true;
            shaftRenderer.material = shaftMaterial;
            shaftRenderer.receiveShadows = false;
            shaftRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            shaftRenderer.SetWidth(locationOffset.magnitude, shaftWidth);
            shaftRenderer.SetColors(lightColor, Color.black);

            Vector3 origin = initialLocation - (locationOffset * i);
            Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (i * RaySpacing + initialAngle)) * lightDistance, Mathf.Sin(Mathf.Deg2Rad * (i * RaySpacing + initialAngle)) * lightDistance);

            shaftRenderer.SetPosition(0, origin);
            shaftRenderer.SetPosition(1, origin + direction);
        }
    }

    void LateUpdate()
    {
        float RaySpacing = lightCone / numberOfRays;
        float initialAngle = lightAngle - (lightCone / 2);

        Vector3 initialLocation = transform.position + (new Vector3(Mathf.Cos(Mathf.Deg2Rad * (lightAngle - 90)), Mathf.Sin(Mathf.Deg2Rad * (lightAngle - 90))) * lightSourceSize / 2);
        Vector3 locationOffset = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (lightAngle - 90)), Mathf.Sin(Mathf.Deg2Rad * (lightAngle - 90))) * lightSourceSize / numberOfRays;

        for (int i = 0; i < numberOfRays; i++)
        {
            Vector3 origin = initialLocation - (locationOffset * i);
            Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (i * RaySpacing + initialAngle)) * lightDistance, Mathf.Sin(Mathf.Deg2Rad * (i * RaySpacing + initialAngle)) * lightDistance);
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, lightDistance, layerMask);

            float travelledDistance = lightDistance;

            LineRenderer shaftRenderer = shafts[i].GetComponent<LineRenderer>();

            if (hit)
            {
                travelledDistance = hit.distance;
            }

            float distanceFrac = travelledDistance / lightDistance;

            shaftRenderer.SetWidth(locationOffset.magnitude, locationOffset.magnitude + ((shaftWidth - locationOffset.magnitude) * distanceFrac));
            shaftRenderer.SetColors(lightColor, new Color(lightColor.r * (1 - distanceFrac), lightColor.g * (1 - distanceFrac), lightColor.b * (1 - distanceFrac)));

            shaftRenderer.SetPosition(0, origin);
            shaftRenderer.SetPosition(1, origin + (direction * distanceFrac));
        }
    }

	void OnDrawGizmos()
    {
        if (DrawDebugRays)
        {
            float RaySpacing = lightCone / numberOfRays;
            float initialAngle = lightAngle - (lightCone / 2);

            Vector3 initialLocation = transform.position + (new Vector3(Mathf.Cos(Mathf.Deg2Rad * (lightAngle - 90)), Mathf.Sin(Mathf.Deg2Rad * (lightAngle - 90))) * lightSourceSize / 2);
            Vector3 locationOffset = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (lightAngle - 90)), Mathf.Sin(Mathf.Deg2Rad * (lightAngle - 90))) * lightSourceSize / numberOfRays;

            for (int i = 0; i < numberOfRays; i++)
            {
                Vector3 origin = initialLocation - (locationOffset * i);
                Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (i * RaySpacing + initialAngle)) * lightDistance, Mathf.Sin(Mathf.Deg2Rad * (i * RaySpacing + initialAngle)) * lightDistance);
                RaycastHit2D hit = Physics2D.Raycast(origin, direction, lightDistance, layerMask);
                if (hit)
                {
                    Gizmos.DrawRay(origin, (direction * hit.distance / lightDistance));
                }
                else
                {
                    Gizmos.DrawRay(origin, direction);
                }
            }
        }
    }
}
