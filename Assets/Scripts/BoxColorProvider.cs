using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface IBoxColorProvider
{
    ColorType GetRandomActiveColor();
}

public class BoxColorProvider : MonoBehaviour, IBoxColorProvider
{
    [SerializeField] private List<ColorType> activeColors = new List<ColorType> { ColorType.red,ColorType.blue};

    public void ActivateColor(ColorType color)
    {
        if(!activeColors.Contains(color))
            activeColors.Add(color);
    }

    public ColorType GetRandomActiveColor()
    {
        return activeColors[Random.Range(0,activeColors.Count)];
    }
}
