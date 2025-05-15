using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerModel
{
    public Vector2Int gridPosition { get; private set; }

    public PlayerModel(Vector2Int startGrid)
    {
        this.gridPosition = startGrid;
    }

    public void Move(Vector2Int direction)
    {
        this.gridPosition += direction;
    }
}
