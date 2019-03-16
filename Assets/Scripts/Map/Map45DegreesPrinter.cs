/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-21-13:40:56
 *	Vertion: 1.0	
 *
 *	Description:
 *      以地图斜45度方向为X方向，来计算宽高，打印
*/

using System.Collections.Generic;
using UnityEngine;

public class Map45DegreesPrinter : IMapPrinter
{
    private BattleMapSystem mapSystem;

    private Transform tileParent;
    private string[] tileNames;
    private int width;
    private int height;
    private Transform[,] tiles;
    private int lastXStart = 0;
    private int lastZStart = 0;
    private int lastXCount = 0;
    private int lastZCount = 0;
    private bool isFirst = true;

    private Dictionary<string, Stack<Transform>> dicRecoveryObject = new Dictionary<string, Stack<Transform>>();

    
    public Map45DegreesPrinter(BattleMapSystem mapSystem, Transform tileParent)
    {
        width = mapSystem.width;
        height = mapSystem.height;
        tiles = new Transform[width, height];
        this.tileParent = tileParent;
        tileNames = mapSystem.tileNames ;

        for (int i = 0; i < tileNames.Length; i++)
        {
            dicRecoveryObject.Add(tileNames[i], new Stack<Transform>());
        }
    }
    public void PrintMap(int[,] map, int xStart, int xCount, int zStart, int zCount)
    {
        int x = xStart;
        int z = zStart;
        if(!isFirst) RecovertyTiles(map,xStart, xCount, zStart, zCount);

        isFirst = false;

        for (int i = 0; i < xCount; i++)
        {
            for (int j = 0; j < zCount; j++)
            {
                x = xStart + i - j;
                z = zStart + i + j;
                if (x < 0 || x >= width || z < 0 || z >= height) continue;
                if (!tiles[x, z])
                {
                    SetTile(map, x, z);
                }

                if (z + 1 >= height) continue;
                if (!tiles[x, z + 1])
                {
                    SetTile(map,x, z + 1);
                }
            }
        }

        lastXStart = xStart;
        lastZStart = zStart;
        lastXCount = xCount;
        lastZCount = zCount;
    }

    private void SetTile(int[,] map, int x, int z)
    {
        Transform transform;
        string name = tileNames[map[x, z]];
        if (dicRecoveryObject[name].Count > 0)
        {
            transform = dicRecoveryObject[name].Pop();
        }
        else
        {
            transform = PoolManager.Instance.GetObject(name, LoadAssetType.Normal, tileParent).transform;
        }
        
        transform.localRotation = Quaternion.identity;
        transform.localPosition = new Vector3(x, 0, z);
        tiles[x, z] = transform;
    }
    private void RecovertyTiles(int xStart, int xCount, int zStart, int zCount, ref int x, ref int z)
    {
        Vector3 currentOrigin = tileParent.localToWorldMatrix * (new Vector3(xStart, 0, zStart));
        Vector3 temp;
        float widthT = width / Mathf.Sin(45);
        float heightT = height / Mathf.Sin(45);
        for (int i = 0; i < xCount; i++)
        {
            for (int j = 0; j < zCount; j++)
            {
                x = lastXStart + i - j;
                z = lastZStart + i + j;
                if (x < 0 || x >= width || z < 0 || z >= height) continue;
                temp.x = x;
                temp.z = z;
                temp.y = 0;
                temp = tileParent.localToWorldMatrix * temp;
                temp = temp - currentOrigin;
                if (tiles[x, z] && (temp.x < 0 || temp.x >= widthT || temp.z < 0 || temp.z >= heightT))
                {
                    PoolManager.Instance.HideObjet(tiles[x, z].gameObject);
                    tiles[x, z] = null;
                }

                z = z + 1;
                if (z >= height) continue;
                temp.x = x;
                temp.z = z;
                temp.y = 0;
                temp = tileParent.localToWorldMatrix * temp;
                temp = temp - currentOrigin;
                if (tiles[x, z] && (temp.x < 0 || temp.x >= widthT || temp.z < 0 || temp.z >= heightT))
                {
                    PoolManager.Instance.HideObjet(tiles[x, z].gameObject);
                    tiles[x, z] = null;
                }
            }
        }
    }
    private void RecovertyTiles(int[,] map, int xStart, int xCount, int zStart, int zCount)
    {
        int x;
        int z;

        int xEnd = xStart + xCount - zCount;
        int zEnd = zStart + xCount + zCount - 2;

        //y = x + b
        int bStart = zStart - xStart;
        int bEnd = zEnd - xEnd;
        //y = -x + c
        int cStart = zStart + xStart;
        int cEnd = zEnd + xEnd;

        for (int i = 0; i < lastXCount; i++)
        {
            for (int j = 0; j < lastZCount; j++)
            {
                
                x = lastXStart + i - j;
                z = lastZStart + i + j;
                if (x >= 0 && x < width && z >= 0 && z < height && tiles[x, z])
                {
                    if (z - x < bStart || z - x > bEnd || z + x < cStart || z + x > cEnd)
                    {
                        RecoveryTile(map, x, z);
                    }
                }

                z = z + 1;
                if (x < 0 || x >= width || z < 0 || z >= height || !tiles[x, z]) continue;

                if (z - x < bStart || z - x > bEnd || z + x < cStart || z + x > cEnd)
                {
                    RecoveryTile(map,x, z);
                }
            }
        }
    }
    private void RecoveryTile(int[,] map, int x, int z)
    {
        string name = tileNames[map[x, z]];

        dicRecoveryObject[name].Push(tiles[x, z]);
        tiles[x, z].localPosition = Vector3.one * -1000;
        tiles[x, z] = null;
    }
}

