/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-08-14:27:06
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using Pathfinding;
using System;
using UnityEngine;

public class NavigationGridVO
{
    private GridGraph _GridGraph;
    private int _Width;
    private int _Depth;
    private int _NodeSize = 1;
    private string _HeightMaskName;
    private Vector3 _Center = default;
    //private Func<bool>

    public NavigationGridVO(int width, int depth)
    {

        _Width = width;
        _Depth = depth;
        _Center = Vector3.zero ;
        _HeightMaskName = GlobalSetting.LAYER_MASK_NAME_GROUND;

        CreatePathGraphic();
    }
    private void CreatePathGraphic()
    {
        AstarData data = AstarPath.active.data;

        _GridGraph = data.AddGraph(typeof(GridGraph)) as GridGraph;

        _GridGraph.SetDimensions(_Width, _Depth, _NodeSize);
    
        _GridGraph.rotation = Vector3.up * 45;

        _GridGraph.center = RoundVector3(_GridGraph.CalculateTransform().Transform(new Vector3(_Width, 0, _Depth))) - Vector3.right * Mathf.Sin(45);

        _GridGraph.collision.heightMask = LayerMask.GetMask(_HeightMaskName);

        AstarPath.active.Scan();  
    }

    public void SetWalableInfoAllMap(Func<int,int,bool> isWalkableFunc)
    {
        AstarPath.active.AddWorkItem(new AstarWorkItem(ctx =>
        {
            for (int i = 0; i < _Depth; i++)
            {
                for (int j = 0; j < _Width; j++)
                {
                    var node = _GridGraph.GetNode(j, i);
                    node.Walkable = isWalkableFunc(j,i);
                }
            }

            _GridGraph.GetNodes(node => _GridGraph.CalculateConnections((GridNodeBase)node));
        }));
    }

    public void SetWalkableRect(Vector2Int offset, int width, int depth, bool isWalkable = false)
    {
        AstarPath.active.AddWorkItem(new AstarWorkItem(ctx =>
        {
            int row;
            int colum;
            for (int i = 0; i < depth; i++)
            {
                colum = offset.y + i;
                for (int j = 0; j < width; j++)
                {
                    row = offset.x + j;
                    var node = _GridGraph.GetNode(row, colum);
                    node.Walkable = isWalkable;
                    _GridGraph.CalculateConnectionsForCellAndNeighbours(row, colum);
                }
            }
        }));
    }

    public void SetWalkableCircle(Vector2Int offset, int radius, bool isWalkable = false)
    {
        AstarPath.active.AddWorkItem(new AstarWorkItem(ctx =>
        {
            int startX = offset.x;
            int startY = offset.y;

            for (int i = -radius + 1; i < radius; i++)
            {
                if (startX + i < 0) return;
                for (int j = -radius + 1; j < radius; j++)
                {
                    if (startY + j < 0) return;

                    if (Math.Abs(i) + Math.Abs(j) < radius)
                    {
                        var node = _GridGraph.GetNode(i, j);
                        node.Walkable = isWalkable;
                        _GridGraph.CalculateConnectionsForCellAndNeighbours(i, j);
                    }
                }
            }
        }));

        
    }

    public static Vector3 RoundVector3(Vector3 v)
    {
        const int Multiplier = 2;

        if (Mathf.Abs(Multiplier * v.x - Mathf.Round(Multiplier * v.x)) < 0.001f) v.x = Mathf.Round(Multiplier * v.x) / Multiplier;
        if (Mathf.Abs(Multiplier * v.y - Mathf.Round(Multiplier * v.y)) < 0.001f) v.y = Mathf.Round(Multiplier * v.y) / Multiplier;
        if (Mathf.Abs(Multiplier * v.z - Mathf.Round(Multiplier * v.z)) < 0.001f) v.z = Mathf.Round(Multiplier * v.z) / Multiplier;
        return v;
    }
}

