/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-20-14:42:28
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;
using System.Collections.Generic;
using UnityEngine;


public class BattleMapSystem : IBattleSystem<BattleManager>
{
    private IMapPrinter _Printer;
    private IMapCreater _Creator;
    private IMapChecker _Checker;
    private Camera _SceneCamera;
    private Transform _CameraTF;
    private Transform _CameraParentTF;
    private MapVOProxy _MapProxy;
    private int[,] _Maps;

    public GameObject[] tiles;
    public string[] tileNames;
    public bool[] walkable;
    public Transform tileParent;
    public int width;
    public int height;

    //测试数据
    public float[] weights;
    
    public float xOriMax;
    public float yOriMax;
    public float scale = 5f;

    public ushort maxShowX = 100;
    public ushort maxShowY = 50;
    public Vector3Int showBasePoint;

    private Vector3Int _YDelta;
    private Vector3Int _XDelta;
    private Vector3 _LastCameraPosition;

    public BattleMapSystem(IBattleManager manager) : base(manager)
    {
        width = battleManager.mapWidth;
        height = battleManager.mapHeight;

        _YDelta = new Vector3Int(-1, 0, 1);
        _XDelta = new Vector3Int(1, 0, 1);

        //测试数据赋值
        tileNames = new string[] { "Hill", "Gobi", "Plain", "River" };
        weights = new float[] { 100, 100, 100, 100 };
        walkable = new bool[] { false, true, true, true };
        
        xOriMax = 2;
        yOriMax = 2;
        scale = 5f;

        _SceneCamera = GameObject.FindGameObjectWithTag(GlobalSetting.TAG_BATTLE_SCENE_CAMERA_NAME).GetComponent<Camera>();
        _CameraTF = _SceneCamera.transform;
        _CameraParentTF = _CameraTF.parent;
        tileParent = GameObject.FindGameObjectWithTag(GlobalSetting.TAG_TILE_PARENT_NAME).transform;
        tiles = new GameObject[4];
        tiles[0] = ResourcesMgr.Instance.Load<GameObject>("Hill").res;
        tiles[1] = ResourcesMgr.Instance.Load<GameObject>("Gobi").res;
        tiles[2] = ResourcesMgr.Instance.Load<GameObject>("Plain").res;
        tiles[3] = ResourcesMgr.Instance.Load<GameObject>("River").res;

        _Creator = new MapCreator(this);
        _Checker = new MapChecker(this);
        _Printer = new Map45DegreesPrinter(this, tileParent);

        _MapProxy = facade.RetrieveProxy(MapVOProxy.NAME) as MapVOProxy;

        for (int i = 0; i < tileNames.Length; i++)
        {
            PoolManager.Instance.IncreaseObjectCache(tileNames[i], maxShowX * maxShowY * 3);
        }
    }
    public override void Initialize()
    {
        //创建地图
        _Maps = _Creator.CreateMap();
        _MapProxy.hypsometricMap = (_Creator as MapCreator).originMap;
        //检测处理
        _Checker.CheckMap(_Maps);

        _MapProxy.Init(new MapVO(_Maps), walkable, _SceneCamera, tileParent);

        _LastCameraPosition = _CameraTF.position;

        facade.SendNotification(GlobalSetting.Msg_MapCreateComplete, _MapProxy.hypsometricMap);   
    }
    public void PrintMap()
    {
        Vector3Int centerPosition = _MapProxy.ViewPositionToMap(new Vector3(0.5f, 0.5f, 0));

        showBasePoint = centerPosition - _YDelta * (maxShowY / 2) - _XDelta * (maxShowX / 2);

        //打印地图
        _Printer.PrintMap(_Maps, showBasePoint.x, maxShowX, showBasePoint.z, maxShowY);
    }
    public override void Release()
    {
        _Maps = null;
        _MapProxy.ClearMapData();
        _Printer = null;
        _Creator = null;
        _Checker = null;
        _SceneCamera = null;
        _CameraTF = null;
        _CameraParentTF = null;
        _Maps = null;
        tiles = null;
        tileNames = null;
        walkable = null;
        tileParent = null;
        weights = null;
    }
    public override void Update()
    {
        if (battleManager.isBattleOver) return;

        Vector3Int currentFocusePosition = _MapProxy.ViewPositionToMap(new Vector3(0.5f, 0.5f, 0));
        //显示处理
        if (Vector3.SqrMagnitude(_LastCameraPosition - currentFocusePosition) > 25)
        {
            PrintMap();

            _LastCameraPosition = currentFocusePosition;
        }
    }

    /// <summary>
    /// 在地图百分比区域内，获取一个可以走的格子
    /// </summary>
    /// <param name="startXPrec"></param>
    /// <param name="endXPrec"></param>
    /// <param name="startZPrec"></param>
    /// <param name="endZPrec"></param>
    /// <returns></returns>
    public Vector3Int GetWalkableTileInRect(float startXPrec, float endXPrec, float startZPrec, float endZPrec)
    {
        return _MapProxy.GetBlankInRect(startXPrec, endXPrec, startZPrec, endZPrec);
    }
    public void AddBuildingInfo(MainBaseVO vo)
    {
        _MapProxy.SetBuildingInfo(true, vo.tilePositon, vo.rect);
        if (vo.buildingType == E_Building.MainBase)
        {
            MainBaseVO mainBase = vo as MainBaseVO;

            _MapProxy.SetOccupiedInfo(true, mainBase.tilePositon, mainBase.radius);
        }
    }
    public void ChangeMapTileInfos(Vector3Int startPos, sbyte[,] info)
    {
        _MapProxy.ChangeMapInfos(startPos, info);
    }
    [Obsolete("作废，使用聚焦到指定点消息")]
    public void SetCameraPosition(Vector3 positon)
    {
        positon = WorldToCameraPosition(positon);
        _CameraParentTF.position = positon;
    }
    [Obsolete("作废，没有具体使用该方法的逻辑")]
    private Vector3 WorldToCameraPosition(Vector3 positon)
    {
        //positon.Scale(Vector3.right + Vector3.forward);
        positon.y = GetCameraPositon().y;
        return positon;
    }
    [Obsolete("作废，发送相机移动消息，替换该功能")]
    public void MoveCameraToPosition(Vector3 positon)
    {
        positon = positon - _SceneCamera.transform.parent.position;
        positon.y = 0;
        iTween.MoveBy(_CameraParentTF.gameObject, positon, 1.5f);
    }
    [Obsolete("作废，没有调用该方法的具体逻辑了")]
    public Vector3 GetCameraPositon()
    {
        return _CameraParentTF.position;
    }
    [Obsolete("作废，没有调用该方法的具体逻辑了")]
    public void CameraTranslate(Vector3 vector)
    {
        _CameraTF.Translate(vector);
    }
    [Obsolete("作废，没有调用该方法的具体逻辑了")]
    public Vector3 MapPositionToWorld(Vector3 mapPosition)
    {
        return _MapProxy.localToWorldMatrix * mapPosition;
    }
    [Obsolete("作废，没有调用该方法的具体逻辑了")]
    public static Vector3Int ViewPositionToMap(Vector3 veiwPosition, Camera camera, Transform mapParent)
    {
        Vector3 pos = camera.ViewportToWorldPoint(veiwPosition);

        Ray ray = camera.ViewportPointToRay(veiwPosition);

        float angle = Vector3.Angle(ray.direction, Vector3.up * -pos.y);

        Vector3 targetPos = ray.GetPoint(Mathf.Abs(pos.y) / Mathf.Cos(angle));

        targetPos = mapParent.worldToLocalMatrix * targetPos;

        return new Vector3Int((int)targetPos.x, 0, (int)targetPos.z);
    }
    public bool IsCanOccupedRingArea(Vector3Int position, int internalRadius, int outerRadius)
    {
        return _MapProxy.IsCanOccupedRingArea(position, internalRadius, outerRadius);
    }

    #region 算法初始源码
    //private void PrintMap(int[,] maps)
    //{
    //    Transform createdObject;
    //    for (int i = 0; i < width; i++)
    //    {
    //        for (int j = 0; j < height; j++)
    //        {
    //            createdObject = GameObject.Instantiate<GameObject>(tiles[maps[i, j]]).transform;
    //            createdObject.SetParent(TileParent);
    //            createdObject.localPosition = new Vector3(i - width / 2, 0, j - height / 2);
    //            if (i == 0 && j == 0) GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = new Vector3(i - width / 2, 1, j - height / 2);
    //            if (i == width - 1 && j == 0) GameObject.CreatePrimitive(PrimitiveType.Cylinder).transform.position = new Vector3(i - width / 2, 1, j - height / 2);
    //        }
    //    }
    //}

    //private void CreateMap(out int[,] maps)
    //{
    //    float sum = 0;
    //    foreach (var item in weights)
    //    {
    //        sum += item;
    //    }

    //    float[] tileMaxVals = new float[weights.Length];
    //    tileMaxVals[0] = 0;
    //    for (int i = 1; i < weights.Length; i++)
    //    {
    //        tileMaxVals[i] = weights[i - 1] / sum + tileMaxVals[i - 1];

    //    }


    //    maps = new int[width, height];
    //    float xOri = UnityEngine.Random.Range(0, xOriMax);
    //    float yOri = UnityEngine.Random.Range(0, yOriMax);
    //    float scale = 5f;
    //    float perlinVal;
    //    for (int i = 0; i < width; i++)
    //    {
    //        for (int j = 0; j < height; j++)
    //        {
    //            perlinVal = Mathf.PerlinNoise(xOri + i * scale / width, yOri + j * scale / height);
    //            for (int k = tileMaxVals.Length - 1; k >= 0; k--)
    //            {
    //                if (perlinVal >= tileMaxVals[k])
    //                {
    //                    maps[i, j] = k;
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //}

    ////检测地图
    //public void CheckMap(int[,] map)
    //{
    //    List<int> startRun = new List<int>();       //用来记录列团的行开始索引
    //    List<int> endRun = new List<int>();         //用来记录列团的行结束索引
    //    List<int> columRuns = new List<int>();      //用来记录每个列团的列索引 这三个记录是成队出现的
    //    //获取列团信息，设置
    //    int runCount = GetAllColumRunInfo(map, width, height, startRun, endRun, columRuns);

    //    int[] runLabels;
    //    List<KeyValuePair<int, int>> equivalence = new List<KeyValuePair<int, int>>();
    //    SignLabelToAllRuns(startRun, endRun, columRuns, runCount, out runLabels, equivalence, 0);

    //    MergeRunLabel(runLabels, equivalence);

    //    //处理多余联通区域，只剩下一个可行走联通区域
    //    int mainRunLabel = GetMainwalkableArea(startRun, endRun, columRuns, runLabels);

    //    HandleCloseArea(map, width, height, mainRunLabel, startRun, endRun, columRuns, runLabels);
    //}

    ////封闭区间分析算法
    ////获取所有列团
    //public int GetAllColumRunInfo(int[,] map, int width, int height, List<int> startRun, List<int> endRun, List<int> columRun)
    //{
    //    int runsCount = 0;
    //    for (int i = 0; i < width; i++)
    //    {
    //        if (walkable[map[i, 0]])
    //        {
    //            runsCount++;
    //            startRun.Add(0);
    //            columRun.Add(i);
    //        }
    //        for (int j = 1; j < height; j++)
    //        {
    //            if (!walkable[map[i, j - 1]] && walkable[map[i, j]])
    //            {
    //                runsCount++;
    //                startRun.Add(j);
    //                columRun.Add(i);
    //            }
    //            else if (walkable[map[i, j - 1]] && !walkable[map[i, j]])
    //            {
    //                endRun.Add(j - 1);
    //            }
    //        }
    //        if (walkable[map[i, height - 1]])
    //        {
    //            endRun.Add(height - 1);
    //        }
    //    }

    //    return runsCount;
    //}

    ////给每个团进行标号
    //public static void SignLabelToAllRuns(List<int> startRun, List<int> endRun, List<int> columnRun, int runCount, out int[] runLabels, List<KeyValuePair<int, int>> equivalences, int offset)
    //{
    //    runLabels = new int[runCount];

    //    int labelIndex = 1;
    //    int curColumnIdx = 0;                   //当前处理的行索引，初始第0行
    //    int curColumnFirstRunIndex = 0;         //当前团所在列索引
    //    int preColumnFirstRunIndex = 0;         //前一个团的开始索引
    //    int preColumnLastRunIndex = -1;         //前一个团的结束索引      
    //    for (int i = 0; i < runCount; i++)
    //    {
    //        if (columnRun[i] != curColumnIdx)   //行号不同，记开始了一个新航
    //        {
    //            curColumnIdx = columnRun[i];
    //            preColumnFirstRunIndex = curColumnFirstRunIndex;    //换行，将当前列变为上一行
    //            preColumnLastRunIndex = i - 1;
    //            curColumnFirstRunIndex = i;                         //保存当前列第一个索引
    //        }

    //        for (int j = preColumnFirstRunIndex; j <= preColumnLastRunIndex; j++)
    //        {
    //            //判断是否跟上一列的所有团有重合
    //            if (startRun[i] <= endRun[j] + offset && endRun[i] >= startRun[j] - offset && columnRun[i] == columnRun[j] + 1)
    //            {

    //                if (runLabels[i] == 0) // 第一次查找到重合，没有被标号过，直接复制重合的上一列的团
    //                    runLabels[i] = runLabels[j];
    //                else if (runLabels[i] != runLabels[j])// 有过重合的记录， 已经被标号  ，保存等价对，当前标号和重合标号           
    //                    equivalences.Add(new KeyValuePair<int, int>(runLabels[i], runLabels[j])); // 保存等价对
    //            }
    //        }


    //        if (runLabels[i] == 0) //没有与前一列的任何run重合, 增加一个新的标号
    //        {
    //            runLabels[i] = labelIndex++;
    //        }
    //    }
    //}

    ////合并团标号
    //public static void MergeRunLabel(int[] runLabels, List<KeyValuePair<int, int>> equivalence)
    //{
    //    int maxLabel = GetMax(runLabels);
    //    bool[][] eqTab = new bool[maxLabel][];
    //    for (int i = 0; i < eqTab.Length; i++)
    //    {
    //        eqTab[i] = new bool[maxLabel];
    //    }
    //    IEnumerator<KeyValuePair<int, int>> enumberator = equivalence.GetEnumerator();

    //    while (enumberator.MoveNext())
    //    {
    //        eqTab[enumberator.Current.Key - 1][enumberator.Current.Value - 1] = true;
    //        eqTab[enumberator.Current.Value - 1][enumberator.Current.Key - 1] = true;
    //    }
    //    int[] labelFlag = new int[maxLabel];
    //    List<List<int>> equaList = new List<List<int>>();
    //    List<int> tempList = new List<int>();

    //    for (int i = 1; i <= maxLabel; i++)
    //    {
    //        if (labelFlag[i - 1] != 0)
    //        {
    //            continue;
    //        }
    //        labelFlag[i - 1] = equaList.Count + 1;
    //        tempList.Add(i);
    //        for (int j = 0; j < tempList.Count; j++)
    //        {
    //            for (int k = 0; k != eqTab[tempList[j] - 1].Length; k++)
    //            {
    //                if (eqTab[tempList[j] - 1][k] && labelFlag[k] == 0)
    //                {
    //                    tempList.Add(k + 1);
    //                    labelFlag[k] = equaList.Count + 1;
    //                }
    //            }
    //        }
    //        equaList.Add(tempList);
    //        tempList.Clear();
    //    }

    //    for (int i = 0; i != runLabels.Length; i++)
    //    {
    //        runLabels[i] = labelFlag[runLabels[i] - 1];
    //    }
    //}

    ////处理封闭区域，用周围的区域抹平
    //public static void HandleCloseArea(int[,] map, int width, int height, int mainRunLabel, List<int> startRun, List<int> endRun, List<int> columRuns, int[] runLabels)
    //{
    //    int modifyVal = -1;
    //    for (int i = 0; i < runLabels.Length; i++)
    //    {
    //        if (runLabels[i] != mainRunLabel)
    //        {
    //            if (startRun[i] > 0)
    //            {
    //                modifyVal = map[columRuns[i], startRun[i] - 1];
    //            }
    //            else if (endRun[i] < height - 1)
    //            {
    //                modifyVal = map[columRuns[i], endRun[i] + 1];
    //            }
    //            else if (columRuns[i] > 0)
    //            {
    //                modifyVal = map[columRuns[i] - 1, startRun[i]];
    //            }
    //            else
    //            {
    //                modifyVal = map[columRuns[i] + 1, startRun[i]];
    //            }

    //            for (int j = startRun[i]; j <= endRun[i]; j++)
    //            {
    //                map[columRuns[i], j] = modifyVal;
    //            }
    //        }

    //    }
    //}

    ////获取数组中的最大值
    //public static int GetMax(int[] list)
    //{
    //    int value = list[0];

    //    for (int i = 1; i < list.Length; i++)
    //    {
    //        if (value < list[i]) value = list[i];
    //    }

    //    return value;
    //}

    ////获取主可行动区域的标号， 占最大面积的
    //public static int GetMainwalkableArea(List<int> startRun, List<int> endRun, List<int> columRuns, int[] runLabels)
    //{
    //    Dictionary<int, int> areaContents = new Dictionary<int, int>();

    //    for (int i = 0; i < runLabels.Length; i++)
    //    {
    //        if (areaContents.ContainsKey(runLabels[i]))
    //        {
    //            areaContents[runLabels[i]] += endRun[i] - startRun[i] + 1;
    //        }
    //        else
    //        {
    //            areaContents.Add(runLabels[i], endRun[i] - startRun[i] + 1);
    //        }
    //    }

    //    KeyValuePair<int, int> mainAreaRunlabels = new KeyValuePair<int, int>(-1, 0);
    //    foreach (var item in areaContents)
    //    {
    //        if (item.Value > mainAreaRunlabels.Value) mainAreaRunlabels = item;
    //    }

    //    return mainAreaRunlabels.Key;
    //}
    #endregion
}

