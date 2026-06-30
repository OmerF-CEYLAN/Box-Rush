using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public struct ColorTypeMapping
{
    public ColorType colorType;
    public Color color;
}

[CreateAssetMenu(fileName = "BoxColorMapping", menuName = "Conveyor/Box Color Mapping")]
public class BoxColorMapping : ScriptableObject
{
    [SerializeField] private List<ColorTypeMapping> mappings;

    public Color GetColor(ColorType colorType)
    {
        foreach (var mapping in mappings)
        {
            if (mapping.colorType == colorType)
                return mapping.color;
        }

        return Color.white;
    }
}