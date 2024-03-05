using System;
using UnityEngine;

[System.Serializable]
public class GridConfiguration
{
    [SerializeField] private int row;
    [SerializeField] private int column;
    [SerializeField] private Vector2 gridCenterPosition;
    [SerializeField] private Vector2 targetScale;
    [SerializeField] private Vector2 offsetBetweenSlots;
    public int Row
    {
        get => row;
        set => row = value;
    }

    public int Column => column;
    public Vector2 GridCenterPosition => gridCenterPosition;
    public Vector2 TargetScale => targetScale;
    public Vector2 OffsetBetweenSlots => offsetBetweenSlots;
    public GridConfiguration(int row, int column, Vector2 gridCenterPosition)
    {
        this.row = row;
        this.column = column;
        this.gridCenterPosition = gridCenterPosition;
    }
}