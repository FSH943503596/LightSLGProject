using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapMediator : Mediator
{
    new public static string NAME = "MiniMapMediator";

    public MiniMapMediator() : base(NAME) { }


    private MiniMapUIForm _MiniMapUIForm;
    private float[,] _MapData;
    private MainBaseVO _MainBaseVO;
    private Vector3Int _MainBasePosition;

    private int _MapWidth = 100;
    private int _MapHeight = 100;
    private int _Scale = 4;
    /// <summary>
    /// 地块大小
    /// </summary>
    private int _PlotSize = 1;
    /// <summary>
    /// 最终生成的柏林噪声图
    /// </summary>
    private Texture2D _NoiseTex;
    /// <summary>
    /// 颜色数组
    /// </summary>
    private Color[] _Pix;
    /// <summary>
    /// 方块的材质
    /// </summary>
    //private MeshRenderer meshRend;

    private Dictionary<GameObject, MainBaseVO> _Dic_MainBaseVO = new Dictionary<GameObject, MainBaseVO>();
    private List<GameObject> _SoldierList = new List<GameObject>();



    public void InitMiniMapMediator()
    {
        if (_MiniMapUIForm == null)
        {
            Debug.Log("InitMiniMapMediator()/_MiniMapUIForm is null");
        }
        if (_MapData == null)
        {
            Debug.Log("InitMiniMapMediator()/_MapData is null");
        }
        _MiniMapUIForm.btnOpenMap.onClick.AddListener(OnClick_OpenMap);
    }

    public override IList<string> ListNotificationInterests()
    {
        return new List<string>()
        {
            GlobalSetting.Msg_InitMiniMapMediator,
            GlobalSetting.Msg_UpdateMainBase,
            GlobalSetting.Msg_MainbaseCreateComplete,
            GlobalSetting.Msg_MapCreateComplete,
            GlobalSetting.Msg_MapUpdateSoldiersPositon,
        };
    }
    int name;
    public override void HandleNotification(INotification notification)
    {
        Debug.Log("进入Mediator，处理消息");

        switch (notification.Name)
        {
            case GlobalSetting.Msg_InitMiniMapMediator:
                Awake();
                _MiniMapUIForm = notification.Body as MiniMapUIForm;
                InitMiniMapMediator();
                Debug.Log("进入Mediator，处理消息 Msg_InitMiniMapMediator");

                break;
            case GlobalSetting.Msg_UpdateMainBase:
                Debug.Log("进入Mediator，处理消息 Msg_UpdateMainBase");

                break;
            case GlobalSetting.Msg_MainbaseCreateComplete:
                Debug.Log("进入Mediator，处理消息 Msg_MainbaseCreateComplete");
                _MainBaseVO = notification.Body as MainBaseVO;
                var _MainBaseGo = _MiniMapUIForm.CreateMainBase();
                var _TilePositon = Position(_MainBaseVO.tilePositon);
                _MainBaseGo.GetComponent<RectTransform>().anchoredPosition = new Vector2(_TilePositon.x, _TilePositon.y);
                var sc = _MainBaseGo.GetComponent<MainBaseDrawEvent>();
                sc.mainBaseVO = _MainBaseVO;
                _Dic_MainBaseVO.Add(_MainBaseGo, _MainBaseVO);
                break;
            case GlobalSetting.Msg_MapCreateComplete:
                Debug.Log("进入Mediator，处理消息 Msg_MapCreateComplete");

                _MapData = notification.Body as float[,];
                _MapWidth = _MapData.GetLength(0) * _Scale;
                _MapHeight = _MapData.GetLength(1) * _Scale;
                break;
            case GlobalSetting.Msg_MapUpdateSoldiersPositon:
                Debug.Log("进入Mediator，处理消息 Msg_MapUpdateSoldiersPositon");
                _MapSoldierInfo = notification.Body as MapSoldierInfo;
                FillTroopsData();
                break;
            default:
                break;
        }
    }
    MapSoldierInfo _MapSoldierInfo;
    private void FillTroopsData()
    {
        int index = -1;
        _SoldierList.ForEach(p => p.SetActive(false));
        _SoldierList.Clear();
        var nextNode = _MapSoldierInfo;
        Debug.Log("nextNode.position ：" + nextNode.position);
        while (nextNode != null)
        {
            if (++index < _SoldierList.Count)
            {
                _SoldierList[index].SetActive(true);
                _SoldierList[index].GetComponent<RectTransform>().anchoredPosition = Position(nextNode.position);
                _SoldierList[index].GetComponent<Image>().color = GlobalSetting.PLAYER_COLOR_LIST[nextNode.coloerIndex];
            }
            else
            {
                var go = _MiniMapUIForm.CreateSoldier();
                go.GetComponent<RectTransform>().anchoredPosition = Position(nextNode.position);
                go.GetComponent<Image>().color = GlobalSetting.PLAYER_COLOR_LIST[nextNode.coloerIndex];
                _SoldierList.Add(go);
            }
            nextNode = nextNode.nextNode;
        }
    }
    private Vector3 Position(Vector3 pos)
    {
        return new Vector3(pos.x * _Scale - _MapWidth  / 2, pos.z * _Scale - _MapHeight  / 2,0);
    }

    private void OnClick_MainBaseMoveTroops(GameObject go)
    {
        MainBaseVO _MainBase = null;
        _Dic_MainBaseVO.TryGetValue(go, out _MainBase);

    }

    private bool IsDisplayMap = false;
    private void OnClick_OpenMap()
    {
        Debug.Log("进入Mediator，点击按钮事件触发");
        IsDisplayMap = !IsDisplayMap;
        if (IsDisplayMap == false)
        {
            _SoldierList.ForEach(p=> GameObject.Destroy(p));
        }
        _MiniMapUIForm.imgMap.gameObject.SetActive(IsDisplayMap);
        FillMapData();
    }

    private void FillMapData()
    {
        Debug.Log("进入Mediator，填充地图数据");       

        //meshRend = GetComponent<MeshRenderer>();
        _NoiseTex = new Texture2D(_MapWidth, _MapHeight);
        // 根据图片的宽高填充颜色数组
        _Pix = new Color[_NoiseTex.width * _NoiseTex.height];
        // 将生成的柏林噪声图赋值给方块的材质
        //meshRend.material.mainTexture = noiseTex;

        _MiniMapUIForm.imgMap.rectTransform.sizeDelta = new Vector2(_NoiseTex.width, _NoiseTex.height);

        Sprite sp = Sprite.Create(_NoiseTex, new Rect(0, 0, _NoiseTex.width, _NoiseTex.height), new Vector2(0.5f, 0.5f));

        _MiniMapUIForm.imgMap.sprite = sp;

        FillPlotData();
    }

    /// <summary>
    /// 填充地块数据
    /// </summary>
    private void FillPlotData()
    {
        Debug.Log("进入Mediator，填充地块数据");

        int y = 0;
        int max_x = 0;
        int max_y = 0;

        while (y < _MapHeight)
        {
            int x = 0;
            int w = 0;
            int h = 0;
            while (x < _MapWidth)
            {
                //// 计算出X的采样值
                //float xCoord = _XOrg + x / _NoiseTex.width * _Scale;
                //// 计算出Y的采样值
                //float yCoord = _YOrg + y / _NoiseTex.height * _Scale;
                //// 用计算出的采样值计算柏林噪声
                //float sample = Mathf.PerlinNoise(xCoord, yCoord);
                Color color = Color.white;


                if (x % _Scale == 0 && x != 0)
                {
                    max_x++;
                }
                if (max_x >= _MapData.GetLength(0))
                {
                    continue;
                }
           
                foreach (var item in GetColorByHeight_Dic)
                {
                    if (_MapData[max_x, max_y] < item.Key)
                    {

                        GetColorByHeight_Dic.TryGetValue(item.Key, out color);
                        break;
                    }
                }

                // 填充颜色数组
                w = _PlotSize > _NoiseTex.width - (int)x ? _NoiseTex.width - (int)x : _PlotSize;
                h = _PlotSize > _NoiseTex.height - (int)y ? _NoiseTex.height - (int)y : _PlotSize;

                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        _Pix[Convert.ToInt32((i + y) * _NoiseTex.width + j + x)] = color;
                    }
                }
                for (int i = (int)x; i < w; i++)
                {
                    for (int j = (int)y; j < h; j++)
                    {
                        _Pix[Convert.ToInt32(j * _NoiseTex.width + i)] = color;
                    }
                }
                //pix[Convert.ToInt32(y * noiseTex.width + x)] = color;

                //x += w;
                x++;

            }
            if (y % _Scale == 0 && y != 0)
            {
                max_y++;
            }
            if (max_y >= _MapData.GetLength(1))
            {
                break;
            }
            max_x = 0;
            //y += h;
            y++;

        }
        _NoiseTex.SetPixels(_Pix);
        _NoiseTex.Apply();


    }

    Dictionary<float, Color> GetColorByHeight_Dic = new Dictionary<float, Color>();
    private void Awake()
    {
        GetColorByHeight_Dic.Add(0.2f, GetColorByValue(150, 190, 250));

        GetColorByHeight_Dic.Add(0.4f, GetColorByValue(240, 230, 100));

        GetColorByHeight_Dic.Add(0.6f, GetColorByValue(30, 162, 70));
        GetColorByHeight_Dic.Add(0.8f, GetColorByValue(140, 70, 50));
        GetColorByHeight_Dic.Add(1f, GetColorByValue(240, 230, 100));

    }
    Color GetColorByValue(int r, int g, int b)
    {
        return new Color(r / 255f, g / 255f, b / 255f);
    }
}
