/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-20-16:02:37
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;
using UnityEngine;

public class MapCreator : IMapCreater
{
    BattleMapSystem mapSystem;

    private float[] weights;
    private int width;
    private int height;
    private float xOriMax;
    private float yOriMax;
    private float scale = 5f;

    private float totalWeight;
    private float[] tileMaxVals;

    public MapCreator(BattleMapSystem mapSystem)
    {
        this.mapSystem = mapSystem;

        //测试数据
        weights = mapSystem.weights;
        width = mapSystem.width;
        height = mapSystem.height;
        xOriMax = mapSystem.xOriMax;
        yOriMax = mapSystem.yOriMax;
        scale = mapSystem.scale;

        totalWeight = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            totalWeight += weights[i];
        }

        tileMaxVals = new float[weights.Length];
        tileMaxVals[0] = 0;
        for (int i = 1; i < weights.Length; i++)
        {
            tileMaxVals[i] = weights[i - 1] / totalWeight + tileMaxVals[i - 1];
        }
    }

    public int[,] CreateMap()
    {             
        int[,] maps = new int[width, height];
        float xOri = UnityEngine.Random.Range(0, xOriMax);
        float yOri = UnityEngine.Random.Range(0, yOriMax);
        float perlinVal;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                perlinVal = Mathf.PerlinNoise(xOri + i * scale / width, yOri + j * scale / height);
                for (int k = tileMaxVals.Length - 1; k >= 0; k--)
                {
                    if (perlinVal >= tileMaxVals[k])
                    {
                        maps[i, j] = k;
                        break;
                    }
                }
            }
        }

        return maps;
    }
}

