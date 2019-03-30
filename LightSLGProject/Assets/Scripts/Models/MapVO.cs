/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-01-10:02:32
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;

public class MapVO
{
    private int height;
    private int width;
    private int[,] data;
    private bool[,] isOccupied;             //该格子是否被基地占领了
    private bool[,] isBuilding;             //该格子是否有建筑

    public int Height { get => height; }
    public int Width { get => width;}

    public MapVO() { }
    public MapVO(int width, int height)
    {
        this.width = width;
        this.height = height;
        data = new int[width, height];
        isOccupied = new bool[width, height];
        isBuilding = new bool[width, height];
    }
    public MapVO(int[,] mapData)
    {
        width = mapData.GetLength(0);
        height = mapData.GetLength(1);
        data = mapData;
        isOccupied = new bool[width, height];
        isBuilding = new bool[width, height];
    }

    public int GetValue(int x, int z)
    {
        return data[x, z];
    }

    private void SetValue(int x, int z, int value)
    {
        data[x, z] = value;
    }

    public bool IsOccupied(int x, int z)
    {
        return isOccupied[x, z];
    }

    public void SetOccupied(int x, int z, bool isOccupied)
    {
        this.isOccupied[x, z] = isOccupied;
    }

    public bool IsBuilding(int x, int z)
    {
        return isBuilding[x, z];
    }

    public void SetBuilding(int x, int z, bool isBuilding)
    {
        this.isBuilding[x, z] = isBuilding;
    }

    public void ChangeMapInfos(int posX, int posZ, sbyte[,] info)
    {
        int infoWidth = info.GetLength(0);
        int infoHeight = info.GetLength(1);
        if (posX < 0 || posX >= width || posZ < 0 || posZ >= height) return;

        int x;
        int z;
        for (int i = 0; i < infoWidth; i++)
        {
            x = posX + i;
            if (x < 0 || x >= width) continue;
            for (int j = 0; j < infoHeight; j++)
            {
                z = posZ + j;
                if (info[i, j] == -1 || z < 0 || z >= height) continue;
                data[x, z] = info[i, j];
            }
        }
    }
}

