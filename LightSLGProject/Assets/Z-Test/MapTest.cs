using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTest : MonoBehaviour
{
    /// <summary>
    /// 图片的宽度
    /// </summary>
    [SerializeField] private int pictureWidth = 100;
    /// <summary>
    /// 图片的高度
    /// </summary>
    [SerializeField] private int pictrueHeight = 100;
    /// <summary>
    /// 用于柏林噪声的X采样偏移量（仿伪随机）
    /// </summary>
    [SerializeField] private float xOrg = .0f;
    /// <summary>
    /// 用于柏林噪声的Y采样偏移量（仿伪随机）
    /// </summary>
    [SerializeField] private float yOrg = .0f;
    /// <summary>
    /// 柏林噪声的缩放值（值越大，柏林噪声计算越密集）
    /// </summary>
    [SerializeField] private float scale = 4f;
    /// <summary>
    /// 地块大小
    /// </summary>
    [SerializeField] public int PlotSize = 4;

    /// <summary>
    /// 最终生成的柏林噪声图
    /// </summary>
    private Texture2D noiseTex;
    /// <summary>
    /// 颜色数组
    /// </summary>
    private Color[] pix;
    /// <summary>
    /// 方块的材质
    /// </summary>
    private MeshRenderer meshRend;


    private void Start()
    {
        meshRend = GetComponent<MeshRenderer>();
        noiseTex = new Texture2D(pictureWidth, pictrueHeight);
        // 根据图片的宽高填充颜色数组
        pix = new Color[noiseTex.width * noiseTex.height];
        // 将生成的柏林噪声图赋值给方块的材质
        //meshRend.material.mainTexture = noiseTex;

        GetComponent<RectTransform>().sizeDelta = new Vector2(noiseTex.width, noiseTex.height);

        Sprite sp = Sprite.Create(noiseTex, new Rect(0, 0, noiseTex.width, noiseTex.height), new Vector2(0.5f, 0.5f));

        GetComponent<Image>().sprite = sp;
    }

    private void Update()
    {
        // 计算柏林噪声
        CalcNoise();
    }

    /// <summary>
    /// 计算柏林噪声
    /// </summary>
    private void CalcNoise()
    {
        float y = .0f;
        while (y < noiseTex.height)
        {
            float x = .0f;
            int w = 0;
            int h = 0;

            while (x < noiseTex.width)
            {
                // 计算出X的采样值
                float xCoord = xOrg + x / noiseTex.width * scale;
                // 计算出Y的采样值
                float yCoord = yOrg + y / noiseTex.height * scale;
                // 用计算出的采样值计算柏林噪声
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                Color color = Color.white;

              
                foreach (var item in GetColorByHeight_Dic)
                {
                    if (sample < item.Key)
                    {
                        
                        GetColorByHeight_Dic.TryGetValue(item.Key, out color);
                        break;
                    }
                }
                
                // 填充颜色数组
                w = PlotSize > noiseTex.width - (int)x ? noiseTex.width - (int)x : PlotSize;
                h = PlotSize > noiseTex.height - (int)y ? noiseTex.height - (int)y : PlotSize;

                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        pix[Convert.ToInt32((i + y) * noiseTex.width + j + x)] = color;
                    }
                }
                for (int i = (int)x; i < w; i++)
                {
                    for (int j = (int)y; j < h; j++)
                    {
                        pix[Convert.ToInt32(j * noiseTex.width + i)] = color;
                    }
                }
                //pix[Convert.ToInt32(y * noiseTex.width + x)] = color;

                x+=w;
            }
            y+=h;
        }
        noiseTex.SetPixels(pix);
        noiseTex.Apply();


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
    Color GetColorByValue(int r,int g,int b)
    {
        return new Color(r / 255f, g / 255f, b / 255f);
    }
}

