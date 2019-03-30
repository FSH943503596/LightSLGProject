/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-22-17:00:39
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;
using UnityEngine;

public abstract class IBuildingVO
{
    protected string _PrefabName = "Building";
    protected Vector3Int _TilePositon;
    protected Vector3 _Postion;
    protected RectInt _Rect = new RectInt(0, 0, 4, 1);
    protected E_Building _BuildingType = E_Building.None;
    protected int _RotateValue = 0;

    public abstract ushort createCostGold {get; }
    public abstract ushort createCostGrain { get; }
    public Vector3Int tilePositon { get => _TilePositon; set => _TilePositon = value; }
    public RectInt rect { get => _Rect; set => _Rect = value; }
    public string prefabName { get => _PrefabName; set => _PrefabName = value; }
    public E_Building buildingType { get => _BuildingType; set => _BuildingType = value; }
    public int rotateValue { get => _RotateValue; }
    public Vector3 postion { get => _Postion; set => _Postion = value; }
    public void Rotate()
    {
        if (_RotateValue == 0)
        {
            _RotateValue = 270;
            _Rect = new RectInt(new Vector2Int(_Rect.size.y - _Rect.position.y - 1, _Rect.position.x), 
                               new Vector2Int(_Rect.size.y, _Rect.size.x));
        }
        else
        {
            _RotateValue = 0;
            _Rect = new RectInt(new Vector2Int(_Rect.position.y, _Rect.size.x - _Rect.position.x - 1),
                               new Vector2Int(_Rect.size.y, _Rect.size.x));
        }
    }
}

