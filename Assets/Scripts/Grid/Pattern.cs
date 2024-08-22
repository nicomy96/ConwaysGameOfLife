using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Pattern", menuName = "Grid/Pattern")]
public class Pattern : ScriptableObject
{
    [SerializeField] int height;
    [SerializeField] int width;
    [SerializeField] List<bool> activeTiles;
    int pivotIndex = 0;

    public int Height
    {
        get { return height; }
    }

    public int Width
    {
        get { return width; }
    }

    public int Pivot
    {
        get { return pivotIndex; }
        set
        {
            if (value >= height * width || value < 0) return;
            pivotIndex = value;
        }
    }

    public bool IsAWellDefinedPattern()
    {
        return activeTiles.Count == height * width;
    }
    public void RestartPivot()
    {
        pivotIndex = 0;
    }

    public int LeftTilesNeeded()
    {
        return pivotIndex % width;
    }

    public int RightTilesNeeded()
    {
        return width - LeftTilesNeeded() - 1;
    }

    public int TopTilesNeeded()
    {
        return Mathf.FloorToInt(pivotIndex / width);
    }

    public int BottomTilesNeeded()
    {
        return height - TopTilesNeeded() - 1;
    }

    public bool IsActiveTile(int index)
    {
        return activeTiles[index];
    }
}
