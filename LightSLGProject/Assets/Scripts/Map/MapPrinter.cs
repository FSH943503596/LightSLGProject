/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-20-17:07:32
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using UnityEngine;

public class MapPrinter : IMapPrinter
{
    private BattleMapSystem battleMapSystem;
    private Transform tileParent;
    private GameObject[] tiles;
    private string[] tileNames;
    private int width;
    private int height;

    public MapPrinter(BattleMapSystem battleMapSystem, Transform tileParent)
    {
        this.tileParent = tileParent;
        this.battleMapSystem = battleMapSystem;
        this.tiles = battleMapSystem.tiles;
        this.width = battleMapSystem.width;
        this.height = battleMapSystem.height;
        this.tileNames = battleMapSystem.tileNames;
    }

    public void PrintMap(int[,] map, int xStart, int xCount, int zStart, int zCount)
    {
        Transform createdObject;
        for (int i = xStart; i < xCount; i++)
        {
            for (int j = zStart; j < zCount; j++)
            {
                //createdObject = GameObject.Instantiate<GameObject>(tiles[map[i, j]]).transform;
                createdObject = PoolManager.Instance.GetObject(tileNames[map[i, j]], LoadAssetType.Normal, tileParent).transform;
                createdObject.localPosition = new Vector3(i - width / 2, 0, j - height / 2);
            }
        }
    }
}

