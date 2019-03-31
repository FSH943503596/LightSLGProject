using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapMediator : Mediator
{
    new public static string NAME = "MiniMapMediator";

    public MiniMapMediator() : base(NAME) { }


    private MiniMapUIForm _MiniMapUIForm;
    private float[,] _MapData;
    private MainBaseVO _MainBaseVO;

    private int _MapWidth = 100;
    private int _MapHeight = 100;
    /// <summary>
    /// ���ڰ���������X����ƫ��������α�����
    /// </summary>
    private float _XOrg = .0f;
    /// <summary>
    /// ���ڰ���������Y����ƫ��������α�����
    /// </summary>
    private float _YOrg = .0f;
    /// <summary>
    /// ��������������ֵ��ֵԽ�󣬰�����������Խ�ܼ���
    /// </summary>
    private float _Scale = 4f;
    /// <summary>
    /// �ؿ��С
    /// </summary>
    private int _PlotSize = 1;
    /// <summary>
    /// �������ɵİ�������ͼ
    /// </summary>
    private Texture2D _NoiseTex;
    /// <summary>
    /// ��ɫ����
    /// </summary>
    private Color[] _Pix;
    /// <summary>
    /// ����Ĳ���
    /// </summary>
    //private MeshRenderer meshRend;

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
            GlobalSetting.Msg_MapCreateComplete,
        };
    }

    public override void HandleNotification(INotification notification)
    {
        Debug.Log("����Mediator��������Ϣ");

        switch (notification.Name)
        {
            case GlobalSetting.Msg_InitMiniMapMediator:
                Awake();
                _MiniMapUIForm = notification.Body as MiniMapUIForm;
                InitMiniMapMediator();
                Debug.Log("����Mediator��������Ϣ Msg_InitMiniMapMediator");

                break;
            case GlobalSetting.Msg_UpdateMainBase:
                _MainBaseVO = notification.Body as MainBaseVO;
                Debug.Log("����Mediator��������Ϣ Msg_UpdateMainBase");

                break;
            case GlobalSetting.Msg_MapCreateComplete:
                Debug.Log("����Mediator��������Ϣ Msg_MapCreateComplete");

                _MapData = notification.Body as float[,];

                break;
            default:
                break;
        }
    }

    private bool IsDisplayMap = false;
    private void OnClick_OpenMap()
    {
        Debug.Log("����Mediator�������ť�¼�����");
        IsDisplayMap = !IsDisplayMap;
        _MiniMapUIForm.imgMap.gameObject.SetActive(IsDisplayMap);
        FillMapData();

    }
    int max = 4;
    private void FillMapData()
    {
        Debug.Log("����Mediator������ͼ����");

        _MapWidth = _MapData.GetLength(0) * max;
        _MapHeight = _MapData.GetLength(1) * max;

        //meshRend = GetComponent<MeshRenderer>();
        _NoiseTex = new Texture2D(_MapWidth, _MapHeight);
        // ����ͼƬ�Ŀ�������ɫ����
        _Pix = new Color[_NoiseTex.width * _NoiseTex.height];
        // �����ɵİ�������ͼ��ֵ������Ĳ���
        //meshRend.material.mainTexture = noiseTex;

        _MiniMapUIForm.imgMap.GetComponent<RectTransform>().sizeDelta = new Vector2(_NoiseTex.width, _NoiseTex.height);

        Sprite sp = Sprite.Create(_NoiseTex, new Rect(0, 0, _NoiseTex.width, _NoiseTex.height), new Vector2(0.5f, 0.5f));

        _MiniMapUIForm.imgMap.sprite = sp;

        FillPlotData();
    }

    //private void Update()
    //{
    //    // �����������
    //    CalcNoise();
    //}

    /// <summary>
    /// ���ؿ�����
    /// </summary>
    private void FillPlotData()
    {
        Debug.Log("����Mediator�����ؿ�����");

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
                //// �����X�Ĳ���ֵ
                //float xCoord = _XOrg + x / _NoiseTex.width * _Scale;
                //// �����Y�Ĳ���ֵ
                //float yCoord = _YOrg + y / _NoiseTex.height * _Scale;
                //// �ü�����Ĳ���ֵ�����������
                //float sample = Mathf.PerlinNoise(xCoord, yCoord);
                Color color = Color.white;


                if (x % max == 0 && x != 0)
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

                // �����ɫ����
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
            if (y % max == 0 && y != 0)
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
