using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Pattern", menuName = "Grid/Pattern")]
public class PatternInfo : ScriptableObject
{
    [SerializeField] Sprite patternImage;
    [SerializeField] string name;
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

    public Sprite GetSprite()
    {
        return patternImage;
    }

    public string Name
    {
        get
        {
            return name;
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
