/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-01-09:59:34
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapVOProxy : Proxy
{
    new public const string NAME = "MapVOProxy";
    private bool[] _Walkable;
    private NavigationGridVO _NavigationGridVO;
    private int _Width;
    private int _Height;
    private Camera _SceneCam;
    private Transform _MapParent;
    private float[,] _HypsometricMap;

    public MapVO mapData
    {
        get
        {
            return base.m_data as MapVO;
        }
    }
    public Matrix4x4 localToWorldMatrix { get => _MapParent.localToWorldMatrix; }
    public float[,] hypsometricMap { get => _HypsometricMap; set => _HypsometricMap = value; }

    public MapVOProxy() : base(NAME) { }
    public void Init(MapVO mapData, bool[] walkable, Camera sceneCam, Transform mapParent)
    {
        if (mapData != null && walkable != null)
        {
            base.m_data = mapData;
            this._Walkable = walkable;

            _Width = mapData.Width;
            _Height = mapData.Height;

            this._SceneCam = sceneCam;
            this._MapParent = mapParent;

            _NavigationGridVO = new NavigationGridVO(_Width, _Height);
            _NavigationGridVO.SetWalableInfoAllMap(IsWalkable);
        }
    }
    public bool IsWalkable(int x, int z)
    {
        MapVO data = mapData;
        return !data.IsOccupied(x, z) && _Walkable[mapData.GetValue(x, z)];
    }
    public bool IsWalkable(Vector3Int position)
    {
        return IsWalkable(position.x, position.z);
    }
    public bool IsBlank(int x, int z)
    {
        return !mapData.IsBuilding(x, z) && _Walkable[mapData.GetValue(x, z)];
    }
    /// <summary>
    /// 格子上是否有建筑
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public bool IsBuilding(int x, int z)
    {
        return mapData.IsBuilding(x, z);
    }
    public void ClearMapData()
    {
        m_data = null;
        _Walkable = null;
    }
    public void ChangeMapInfos(Vector3Int startPos, sbyte[,] info)
    {
        mapData.ChangeMapInfos(startPos.x, startPos.z, info);
    }
    public void SetBuildingInfo(bool isBuilding, Vector3Int position, RectInt rect)
    {
        int startX = position.x - rect.position.x;
        int startZ = position.z - rect.position.y;

        if (startX < 0 || startX + rect.size.x > _Width || startZ < 0 || startZ + rect.size.y > _Width) return;

        MapVO mapVO = mapData;
        for (int i = 0; i < rect.size.x; i++)
        {
            for (int j = 0; j < rect.size.y; j++)
            {
                mapVO.SetBuilding(startX + i, startZ + j, isBuilding);
            }
        }
    }
    public void SetOccupiedInfo(bool isOccupied, Vector3Int position, int radius)
    {
        int startX = position.x;
        int startY = position.z;
        MapVO data = mapData;

        for (int i = -radius + 1; i < radius; i++)
        {
            if (startX + i < 0) return;
            for (int j = -radius + 1; j < radius; j++)
            {
                if (startY + j < 0) return;

                if (Math.Abs(i) + Math.Abs(j) < radius)
                {
                    data.SetOccupied(startX + i, startY + j, isOccupied);
                }
            }
        }
    }
    public Vector3Int ViewPositionToMap(Vector3 veiwPosition)
    {
        veiwPosition.z = _SceneCam.nearClipPlane;      
        Ray ray = _SceneCam.ViewportPointToRay(veiwPosition);
        float angle = Vector3.Angle(ray.direction, -Vector3.up);
        Vector3 targetPos = ray.GetPoint(Mathf.Abs(ray.origin.y) * Mathf.Tan(angle * Mathf.Deg2Rad));
        targetPos.y = 0;       
        targetPos = _MapParent.worldToLocalMatrix * targetPos;

        return new Vector3Int((int)targetPos.x, 0, (int)targetPos.z);
    }
    public bool IsCanOccupedArea(Vector3Int position, int radius)
    {
        int startX = position.x;
        int startY = position.z;
        MapVO data = mapData;

        for (int i = -radius + 1; i < radius; i++)
        {
            if (startX + i < 0) return false;
            for (int j = -radius + 1; j < radius; j++)
            {
                if (startY + j < 0) return false;

                if (Math.Abs(i) + Math.Abs(j) < radius)
                {
                    if (data.IsOccupied(startX + i, startY + j)) return false;
                }
            }
        }

        return true;
    }
    public bool IsCanOccupedRingArea(Vector3Int position, int internalRadius, int outerRadius)
    {
        int startX = position.x;
        int startY = position.z;
        int length = 0;
        MapVO data = mapData;

        for (int i = -outerRadius + 1; i < outerRadius; i++)
        {
            if (startX + i < 0) return false;
            for (int j = -outerRadius + 1; j < outerRadius; j++)
            {
                if (startY + j < 0) return false;

                length = Math.Abs(i) + Math.Abs(j);
                if (length < outerRadius &&  length >= internalRadius)
                {
                    if (data.IsOccupied(startX + i, startY + j)) return false;
                }
            }
        }

        return true;
    }

    #region 获取之地区域内的空格子算法
    int startX, endX, startZ, endZ;
    public Vector3Int GetBlankInRect(float startXPrec, float endXPrec, float startZPrec, float endZPrec)
    {
        Vector3Int result = new Vector3Int();

        startX = (int)(_Width * startXPrec);
        endX = (int)(_Width * endXPrec);
        startZ = (int)(_Height * startZPrec);
        endZ = (int)(_Height * endZPrec);

        result.x = startX + (endX - startX) / 2;
        result.z = startZ + (endZ - startZ) / 2;

        List<Vector3Int> needCheck = new List<Vector3Int>();
        List<Vector3Int> isChecked = new List<Vector3Int>();
        needCheck.Add(result);
        while (needCheck.Count > 0)
        {
            result = needCheck[0];
            if (IsBlank(result.x, result.z))
            {
                return result;
            }
            AddNeedCheckedTiles(result, needCheck, isChecked);
        }

        result.y = -1;
        return result;
    }
    private void AddNeedCheckedTiles(Vector3Int result, List<Vector3Int> needCheck, List<Vector3Int> isChecked)
    {
        Vector3Int temp = new Vector3Int();

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                temp.x = result.x + i;
                temp.z = result.z + j;

                if (temp.x >= startX && temp.x <= endX && temp.z >= startZ && temp.z <= endZ && !needCheck.Contains(temp) && !isChecked.Contains(temp))
                {
                    needCheck.Add(temp);
                }
            }
        }

        needCheck.Remove(result);
        isChecked.Add(result);
    }
    #endregion 
}