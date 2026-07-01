using System.Collections.Generic;
using UnityEngine;

public class BoxTargetArea : MonoBehaviour
{
    public Dictionary<Vector3, bool> targetAreaGrids;
    public ColorType colorType;
    public Direction direction;

    [SerializeField] private MonoBehaviour boxRepositorySource;
    private IBoxWriter boxWriter;

    private void Awake()
    {
        boxWriter = boxRepositorySource as IBoxWriter;
        boxWriter.RegisterTargetArea(this);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
