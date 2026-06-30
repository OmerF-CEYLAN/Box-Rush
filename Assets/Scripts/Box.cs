using UnityEngine;

public enum ColorType { red, blue, yellow, green }

public class Box : MonoBehaviour
{
    public ColorType colorType;
    public BoxTargetArea targetArea;
    public MeshRenderer boxRenderer;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTargetArea(BoxTargetArea target)
    {
        targetArea = target;
    }

    public void SetMaterialColor(ColorType colorType)
    {
        switch (colorType)
        {
            case ColorType.red:
                boxRenderer.material.color = Color.red;
                break;
            case ColorType.blue:
                boxRenderer.material.color = Color.blue;
                break;
            case ColorType.yellow:
                boxRenderer.material.color = Color.yellow;
                break;
            case ColorType.green:
                boxRenderer.material.color = Color.green;
                break;
        }
    }
}
