using SUIFW;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapUIForm : BaseUIForm
{
    [SerializeField] private Button _BtnOpenMap;
    [SerializeField] private Image _ImgMap;

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
    private int _PlotSize = 4;
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

    public Button btnOpenMap { get { return _BtnOpenMap; } }
    public float[,] mapData { get; set; }

    private void Start()
    {
        //meshRend = GetComponent<MeshRenderer>();
        _NoiseTex = new Texture2D(mapData.GetLength(0), mapData.GetLength(1));
        // ����ͼƬ�Ŀ�������ɫ����
        _Pix = new Color[_NoiseTex.width * _NoiseTex.height];
        // �����ɵİ�������ͼ��ֵ������Ĳ���
        //meshRend.material.mainTexture = noiseTex;

        GetComponent<RectTransform>().sizeDelta = new Vector2(_NoiseTex.width, _NoiseTex.height);

        Sprite sp = Sprite.Create(_NoiseTex, new Rect(0, 0, _NoiseTex.width, _NoiseTex.height), new Vector2(0.5f, 0.5f));

        _ImgMap.sprite = sp;
    }

    //private void Update()
    //{
    //    // �����������
    //    CalcNoise();
    //}

    /// <summary>
    /// �����������
    /// </summary>
    private void CalcNoise()
    {
        int y = 0;
        while (y < _NoiseTex.height)
        {
            int x = 0;
            int w = 0;
            int h = 0;

            while (x < _NoiseTex.width)
            {
                //// �����X�Ĳ���ֵ
                //float xCoord = _XOrg + x / _NoiseTex.width * _Scale;
                //// �����Y�Ĳ���ֵ
                //float yCoord = _YOrg + y / _NoiseTex.height * _Scale;
                //// �ü�����Ĳ���ֵ�����������
                //float sample = Mathf.PerlinNoise(xCoord, yCoord);
                Color color = Color.white;

                

                foreach (var item in GetColorByHeight_Dic)
                {
                    if (mapData[x, y] < item.Key)
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

                x += w;
            }
            y += h;
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
