/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-18-10:05:51
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/
using System;
using System.Collections.Generic;
using UnityEngine;

public class TileCreator : MonoBehaviour
{
    public GameObject[] tiles;
    public float[] weights;
    public bool[] walkable;
    public int width;
    public int height;   
    public float xOriMax;
    public float yOriMax;
    public float scale = 5f;

    public Transform TileParent;

    void Start()
    {
        int[,] maps;

        //创建地图
        CreateMap(out maps);

        //检测处理
        CheckMap(maps);

        //打印地图
        PrintMap(maps);    
    }

    private void PrintMap(int[,] maps)
    {
        Transform createdObject;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                createdObject = Instantiate<GameObject>(tiles[maps[i, j]]).transform;
                createdObject.SetParent(TileParent);
                createdObject.localPosition = new Vector3(i - width / 2, 0, j - height / 2);
                if (i == 0 && j == 0) GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = new Vector3(i - width / 2, 1, j - height / 2);
                if (i == width - 1 && j == 0) GameObject.CreatePrimitive(PrimitiveType.Cylinder).transform.position = new Vector3(i - width / 2, 1, j - height / 2);
            }
        }
    }

    private void CreateMap(out int[,] maps)
    {
        float sum = 0;
        foreach (var item in weights)
        {
            sum += item;
        }

        float[] tileMaxVals = new float[weights.Length];
        tileMaxVals[0] = 0;
        for (int i = 1; i < weights.Length; i++)
        {
            tileMaxVals[i] = weights[i - 1] / sum + tileMaxVals[i - 1];

        }

        foreach (var item in tileMaxVals)
        {
            print(item);
        }

        maps = new int[width, height];
        float xOri = UnityEngine.Random.Range(0, xOriMax);
        float yOri = UnityEngine.Random.Range(0, yOriMax);
        float scale = 5f;
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
    }

    //检测地图
    public void CheckMap(int[,] map)
    {
        //测试算法
        GetAllWalkAbleArea(map);

        List<RectInt> Areas = new List<RectInt>();

        int checkRowStart = 0;
        int checkRowEnd = height;
        int checkColumnStart = 0;
        int checkColumnEnd = width;

        CaculateAreas(map, Areas, checkRowStart, checkRowEnd, checkColumnStart, checkColumnEnd);

        int index = 0;
        int count = 0;
        while (index < Areas.Count)
        {
            count = Areas.Count;

            CaculateAreas(map, Areas, Areas[index].xMin, Areas[index].xMax, Areas[index].yMin, Areas[index].yMax);

            if (Areas.Count - count == 1)
            {
                Areas[index] = Areas[count];
                Areas.RemoveAt(count);
                index++;
            }
            else
            {
                Areas.RemoveAt(index);
            }
        }

    }

    private void GetAllWalkAbleArea(int[,] map)
    {
        List<int> startRun = new List<int>();       //用来记录列团的行开始索引
        List<int> endRun = new List<int>();         //用来记录列团的行结束索引
        List<int> columRuns = new List<int>();      //用来记录每个列团的列索引 这三个记录是成队出现的
        Debug.unityLogger.logEnabled = false;
        //获取列团信息，设置
        int runCount = GetAllColumRunInfo(map, width, height, startRun, endRun, columRuns);

        for (int i = 0; i < runCount; i++)
        {
            Debug.LogError(string.Format("{0}:{1}->{2}", columRuns[i], startRun[i], endRun[i]));
        }

        int[] runLabels;
        List<KeyValuePair<int, int>> equivalence = new List<KeyValuePair<int, int>>();
        SignLabelToAllRuns(startRun, endRun, columRuns, runCount, out runLabels, equivalence, 0);

        MergeRunLabel(runLabels, equivalence);

        Debug.LogError(startRun.Count);
        Debug.LogError(endRun.Count);
        Debug.LogError(columRuns.Count);

        foreach (var item in runLabels)
        {
            Debug.LogError(item);
        }
        Debug.unityLogger.logEnabled = true;

        //处理多余联通区域，只剩下一个可行走联通区域
        int mainRunLabel =  GetMainwalkableArea(startRun, endRun, columRuns, runLabels);

        HandleCloseArea(map, width, height, mainRunLabel, startRun, endRun, columRuns, runLabels);        
    }   
    
    public void CaculateAreas(int[,] map, List<RectInt> Areas, int checkRowStart, int checkRowEnd, int checkColumnStart, int checkColumnEnd)
    {
        List<Vector2Int> rows = new List<Vector2Int>();
        List<Vector2Int> columns = new List<Vector2Int>();

        bool isNeedCreate = true;
        Vector2Int temp = Vector2Int.zero;
        bool isBreak = false;

        for (int i = checkRowStart; i < checkRowEnd; i++)
        {
            for (int j = checkColumnStart; j < checkColumnEnd; j++)
            {
                if (!walkable[map[i, j]])
                {
                    if (isNeedCreate)
                    {
                        temp.x = i;
                        isNeedCreate = false;
                    }
                    isBreak = true;
                    break;
                }
            }
            if (!isNeedCreate && !isBreak)
            {
                isBreak = false;
                isNeedCreate = true;
                if (i - 1 - temp.x > 1)
                {
                    temp.y = i - 1;
                    rows.Add(temp);
                }

            }
        }

        for (int i = checkColumnStart; i < checkColumnEnd; i++)
        {
            for (int j = checkRowStart; j < checkRowEnd; j++)
            {
                if (!walkable[map[j, i]])
                {
                    if (isNeedCreate)
                    {
                        temp.x = i;
                        isNeedCreate = false;
                    }
                    isBreak = true;
                    break;
                }
            }
            if (!isNeedCreate && !isBreak)
            {
                isBreak = false;
                isNeedCreate = true;
                if (i - 1 - temp.x > 1)
                {
                    temp.y = i - 1;
                    columns.Add(temp);
                }
            }
        }

        for (int i = 0; i < rows.Count; i++)
        {
            for (int j = 0; j < columns.Count; j++)
            {
                Areas.Add(new RectInt(rows[i].x, columns[j].x, rows[i].y, columns[j].y));
            }
        }
    }


    //封闭区间分析算法
    //获取所有列团
    public  int GetAllColumRunInfo(int[,] map, int width, int height, List<int> startRun, List<int> endRun, List<int> columRun)
    {
        int runsCount = 0;
        for (int i = 0; i < width; i++)
        {
            if (walkable[map[i, 0]])
            {
                runsCount++;
                startRun.Add(0);
                columRun.Add(i);
                Debug.LogError(string.Format("map[{0},{1}] = {2} : stRun[{3}] = {4}, rowRun[{5}] = {6}", i, 0, map[i, 0], 0, startRun[0], columRun.Count - 1, columRun[columRun.Count - 1]));
            }
            for (int j = 1; j < height; j++)
            {
                if (!walkable[map[i, j - 1]] && walkable[map[i, j]])
                {
                    runsCount++;
                    startRun.Add(j);
                    columRun.Add(i);
                    Debug.LogError(string.Format("map[{0},{1}] = {2} : stRun[{3}] = {4}, rowRun[{5}] = {6}", i, j, map[i, j], startRun.Count - 1, startRun[startRun.Count - 1], columRun.Count - 1, columRun[columRun.Count - 1]));
                }
                else if (walkable[map[i, j - 1]] && !walkable[map[i, j]])
                {
                    endRun.Add(j - 1);
                    Debug.LogError(string.Format("map[{0},{1}] = {2} : enRun[{3}] = {4}", i, j, map[i, j], endRun.Count - 1, endRun[endRun.Count - 1]));
                }
            }
            if (walkable[map[i, height - 1]])
            {
                endRun.Add(height - 1);
                Debug.LogError(string.Format("map[{0},{1}] = {2} : enRun[{3}] = {4}", i, height - 1, map[i, height - 1], endRun.Count - 1, endRun[endRun.Count - 1]));
            }
        }

        return runsCount;
    }

    //给每个团进行标号
    public static void SignLabelToAllRuns(List<int> startRun, List<int> endRun, List<int> columnRun, int runCount,out int[] runLabels, List<KeyValuePair<int, int>> equivalences, int offset)
    {
        runLabels = new int[runCount];
        
        int labelIndex = 1;         
        int curColumnIdx = 0;                   //当前处理的行索引，初始第0行
        int curColumnFirstRunIndex = 0;         //当前团所在列索引
        int preColumnFirstRunIndex = 0;         //前一个团的开始索引
        int preColumnLastRunIndex = -1;         //前一个团的结束索引      
        for (int i = 0; i < runCount; i++)
        {
            if (columnRun[i] != curColumnIdx)   //行号不同，记开始了一个新航
            {
                curColumnIdx = columnRun[i];
                preColumnFirstRunIndex = curColumnFirstRunIndex;    //换行，将当前列变为上一行
                preColumnLastRunIndex = i - 1;                      
                curColumnFirstRunIndex = i;                         //保存当前列第一个索引
            }

            for (int j = preColumnFirstRunIndex; j <= preColumnLastRunIndex; j++)
            {
                //判断是否跟上一列的所有团有重合
                if (startRun[i] <= endRun[j] + offset && endRun[i] >= startRun[j] - offset && columnRun[i] == columnRun[j] + 1)
                {
                    
                    if (runLabels[i] == 0) // 第一次查找到重合，没有被标号过，直接复制重合的上一列的团
                        runLabels[i] = runLabels[j];
                    else if (runLabels[i] != runLabels[j])// 有过重合的记录， 已经被标号  ，保存等价对，当前标号和重合标号           
                        equivalences.Add(new KeyValuePair<int, int>(runLabels[i], runLabels[j])); // 保存等价对
                }
            }

            
            if (runLabels[i] == 0) //没有与前一列的任何run重合, 增加一个新的标号
            {
                runLabels[i] = labelIndex++;
            }

            Debug.LogError(string.Format("runLabels[{0}]={1}，columnRun[{2}]={3},{4}->{5}", i, runLabels[i], i, columnRun[i], startRun[i], endRun[i]));
        }
    }

    //合并团标号
    public static void MergeRunLabel(int[] runLabels, List<KeyValuePair<int, int>> equivalence)
    {
        int maxLabel = GetMax(runLabels);
        bool[][] eqTab = new bool[maxLabel][];
        for (int i = 0; i < eqTab.Length; i++)
        {
            eqTab[i] = new bool[maxLabel];
        }
        IEnumerator<KeyValuePair<int,int>> enumberator = equivalence.GetEnumerator();

        while (enumberator.MoveNext())
        {
            eqTab[enumberator.Current.Key - 1][enumberator.Current.Value - 1] = true;
            eqTab[enumberator.Current.Value - 1][enumberator.Current.Key - 1] = true;
        }
        int[] labelFlag = new int[maxLabel];
        List<List<int>> equaList = new List<List<int>>();
        List<int> tempList = new List<int>();

        for (int i = 1; i <= maxLabel; i++)
        {
            if (labelFlag[i - 1] != 0)
            {
                continue;
            }
            labelFlag[i - 1] = equaList.Count + 1;
            tempList.Add(i);
            for (int j = 0; j < tempList.Count; j++)
            {
                for (int k = 0; k != eqTab[tempList[j] - 1].Length; k++)
                {
                    if (eqTab[tempList[j] - 1][k] && labelFlag[k] == 0)
                    {
                        tempList.Add(k + 1);
                        labelFlag[k] = equaList.Count + 1;
                    }
                }
            }
            equaList.Add(tempList);
            tempList.Clear();
        }

        for (int i = 0; i != runLabels.Length; i++)
        {
            runLabels[i] = labelFlag[runLabels[i] - 1];
        }
    }

    //处理封闭区域，用周围的区域抹平
    public static void HandleCloseArea(int[,] map, int width, int height, int mainRunLabel, List<int> startRun, List<int> endRun, List<int> columRuns, int[] runLabels)
    {
        int modifyVal = -1;
        for (int i = 0; i < runLabels.Length; i++)
        {
            if (runLabels[i] != mainRunLabel)
            {
                if (startRun[i] > 0)
                {
                    modifyVal = map[columRuns[i], startRun[i] - 1];
                }
                else if (endRun[i] < height - 1)
                {
                    modifyVal = map[columRuns[i], endRun[i] + 1];
                }
                else if (columRuns[i] > 0)
                {
                    modifyVal = map[columRuns[i] - 1, startRun[i]];
                }
                else
                {
                    modifyVal = map[columRuns[i] + 1, startRun[i]];
                }

                for (int j = startRun[i]; j <= endRun[i]; j++)
                {
                    map[columRuns[i], j] = modifyVal;
                }
            }

        }
    }

    //获取数组中的最大值
    public static int GetMax(int[] list)
    {
        int value = list[0];

        for (int i = 1; i < list.Length; i++)
        {
            if (value < list[i]) value = list[i];
        }

        return value;
    }

    //获取主可行动区域的标号， 占最大面积的
    public static int GetMainwalkableArea(List<int> startRun, List<int> endRun, List<int> columRuns, int[] runLabels)
    {
        Dictionary<int, int> areaContents = new Dictionary<int, int>();

        for (int i = 0; i < runLabels.Length; i++)
        {
            if (areaContents.ContainsKey(runLabels[i]))
            {
                areaContents[runLabels[i]] += endRun[i] - startRun[i] + 1;
            }
            else
            {
                areaContents.Add(runLabels[i], endRun[i] - startRun[i] + 1);
            }
        }

        KeyValuePair<int, int> mainAreaRunlabels = new KeyValuePair<int, int>(-1, 0);
        foreach (var item in areaContents)
        {
            if (item.Value > mainAreaRunlabels.Value) mainAreaRunlabels = item;
        }

        return mainAreaRunlabels.Key;
    }
}
