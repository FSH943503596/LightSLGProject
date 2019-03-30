using System;

public static class StaticDataUtility
{
    public const char FIELD_SEGMENTATION_SYMBOL = '|';      //配置表分隔符
    public const float GRID_FLOAT_LENGHT = 0.48F;           //单个格子宽度
    public const int GRID_CONFIG_UNIT = 100;                //配置表代表一格子的数值


    /// <summary>
    /// 将配置信息中格子的单位转化成实际的浮点数
    /// 配置中1格子 = 100
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    public static float ConvertGridUnitToFloat(int grid)
    {
        return grid * GRID_FLOAT_LENGHT / GRID_CONFIG_UNIT;
    }

    /// <summary>
    /// 用来转化表里边的字符的“数组类型的”
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static ushort[] ConvertVariable(string value)
    {
        return ConvertVariable<ushort>(value);
    }

    /// <summary>
    /// 将字符串按照指定分隔符分隔成指定类型的数组
    /// </summary>
    /// <typeparam name="T">返回数组类型</typeparam>
    /// <param name="value">字符串</param>
    /// <param name="segmentationSymbol">分隔符</param>
    /// <returns></returns>
    public static T[] ConvertVariable<T>(string value, char segmentationSymbol = FIELD_SEGMENTATION_SYMBOL)
    {
        string[] TempA = value.Split(segmentationSymbol);
        T[] tempC = new T[TempA.Length];
        for (int i = 0; i < TempA.Length; i++)
        {
            //  Debug.Log(TempA[i]);

            tempC[i] = (T)Convert.ChangeType(TempA[i], typeof(T));
        }
        return tempC;

    }
}
