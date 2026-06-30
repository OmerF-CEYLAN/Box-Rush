using UnityEngine;

public class Box : MonoBehaviour
{
    public ColorType colorType;
    public Direction direction;
    public BoxTargetArea targetArea;
    public MeshRenderer boxRenderer;

    [SerializeField] private BoxColorMapping colorMapping;

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
        boxRenderer.material.color = colorMapping.GetColor(colorType);
    }
}
