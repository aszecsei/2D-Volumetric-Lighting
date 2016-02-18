using UnityEngine;
using System.Collections;

public class LightingCamera : MonoBehaviour {

    protected int width;
    protected int height;
    protected RenderTexture target;
    protected Camera camComponent;

    void Start()
    {
        camComponent = GetComponent<Camera>();
        target = new RenderTexture(Screen.width, Screen.height, 0);
        width = Screen.width;
        height = Screen.height;
        target.name = "TextureFromCamera_" + gameObject.name;
        target.Create();
        camComponent.targetTexture = target;
    }

    void Update()
    {
        if (width != Screen.width || height != Screen.height)
        {
            target.Release();
            Destroy(target);
            target = new RenderTexture(Screen.width, Screen.height, 0);
            width = Screen.width;
            height = Screen.height;
            target.name = "TextureFromCamera_" + gameObject.name;
            target.Create();
            camComponent.targetTexture = target;
        }
    }
}
