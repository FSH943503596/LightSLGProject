/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-20-16:27:07
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System.Collections.Generic;

public class MapChecker : IMapChecker
{
    BattleMapSystem mapSystem;

    private int height;
    private int width;
    private bool[] walkable;

    public MapChecker(BattleMapSystem battleMapSystem)
    {
        mapSystem = battleMapSystem;

        height = mapSystem.height;
        width = mapSystem.width;
        walkable = mapSystem.walkable;
    }

    public void CheckMap(int[,] map)
    {
        List<int> startRun = new List<int>();       //用来记录列团的行开始索引
        List<int> endRun = new List<int>();         //用来记录列团的行结束索引
        List<int> columRuns = new List<int>();      //用来记录每个列团的列索引 这三个记录是成队出现的
                                                    //获取列团信息，设置
        int runCount = GetAllColumRunInfo(map, width, height, startRun, endRun, columRuns);

        int[] runLabels;
        List<KeyValuePair<int, int>> equivalence = new List<KeyValuePair<int, int>>();
        SignLabelToAllRuns(startRun, endRun, columRuns, runCount, out runLabels, equivalence, 0);

        MergeRunLabel(runLabels, equivalence);

        //处理多余联通区域，只剩下一个可行走联通区域
        int mainRunLabel = GetMainwalkableArea(startRun, endRun, columRuns, runLabels);

        HandleCloseArea(map, width, height, mainRunLabel, startRun, endRun, columRuns, runLabels);
    }

    //封闭区间分析算法
    //获取所有列团
    public int GetAllColumRunInfo(int[,] map, int width, int height, List<int> startRun, List<int> endRun, List<int> columRun)
    {
        int runsCount = 0;
        for (int i = 0; i < width; i++)
        {
            if (walkable[map[i, 0]])
            {
                runsCount++;
                startRun.Add(0);
                columRun.Add(i);
            }
            for (int j = 1; j < height; j++)
            {
                if (!walkable[map[i, j - 1]] && walkable[map[i, j]])
                {
                    runsCount++;
                    startRun.Add(j);
                    columRun.Add(i);
                }
                else if (walkable[map[i, j - 1]] && !walkable[map[i, j]])
                {
                    endRun.Add(j - 1);
                }
            }
            if (walkable[map[i, height - 1]])
            {
                endRun.Add(height - 1);
            }
        }

        return runsCount;
    }

    //给每个团进行标号
    public static void SignLabelToAllRuns(List<int> startRun, List<int> endRun, List<int> columnRun, int runCount, out int[] runLabels, List<KeyValuePair<int, int>> equivalences, int offset)
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
        IEnumerator<KeyValuePair<int, int>> enumberator = equivalence.GetEnumerator();

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

