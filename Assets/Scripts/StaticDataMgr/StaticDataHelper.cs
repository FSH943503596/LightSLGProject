
using StaticData.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace StaticData
{
    public static class StaticDataHelper
    {
        private static Dictionary<string, string> _GlobalSettingKeyValue = new Dictionary<string, string>();
        private static Dictionary<int, MainBaseLevelData> _LevelToMainBaseLevelData = new Dictionary<int, MainBaseLevelData>();
        private static Dictionary<int, MainBaseLevelData> _LevelToSubBaseLevelData = new Dictionary<int, MainBaseLevelData>();
        private static int _SubBaseMaxLevel = 0;
        private static int _MainBaseMaxLevel = 0;

        public static int SubBaseMaxLevel => _SubBaseMaxLevel;
        public static int MainBaseMaxLevel => _MainBaseMaxLevel;

        public static Dictionary<int, MainBaseLevelData> LevelToMainBaseLevelData => _LevelToMainBaseLevelData;
        public static Dictionary<int, MainBaseLevelData> LevelToSubBaseLevelData => _LevelToSubBaseLevelData; 

        static StaticDataHelper()
        {
            foreach (var item in StaticDataMgr.mInstance.mGlobalSettingDataMap)
            {
                _GlobalSettingKeyValue.Add(item.Value.Name, item.Value.PropValue);
            }
            StaticDataMgr.mInstance.mGlobalSettingDataMap = null;

            foreach (var item in StaticDataMgr.mInstance.mMainBaseLevelDataMap)
            {
                if (item.Value.MainbaseType == 1)
                {
                    _LevelToMainBaseLevelData.Add(item.Value.Level, item.Value);
                    _MainBaseMaxLevel = _MainBaseMaxLevel < item.Value.Level ? item.Value.Level : _MainBaseMaxLevel;
                }
                else
                {
                    _LevelToSubBaseLevelData.Add(item.Value.Level, item.Value);
                    _SubBaseMaxLevel = _SubBaseMaxLevel < item.Value.Level ? item.Value.Level : _SubBaseMaxLevel;
                }
            }
        }

        public static short[] GetShortArray(string vo)
        {
            string[] temp = vo.Split('|');

            short[] returnValue = new short[temp.Length];

            for (int i = 0; i < temp.Length; i++)
            {
                returnValue[i] = System.Convert.ToInt16(temp[i]);
            }

            return returnValue;
        }
        public static ushort[] GetUshortArray(string vo)
        {
            string[] temp = vo.Split('|');

            ushort[] returnValue = new ushort[temp.Length];

            for (int i = 0; i < temp.Length; i++)
            {
                returnValue[i] = System.Convert.ToUInt16(temp[i]);
            }

            return returnValue;
        }
        public static byte[] GetByteArray(string vo)
        {
            string[] temp = vo.Split('|');

            byte[] returnValue = new byte[temp.Length];

            for (int i = 0; i < temp.Length; i++)
            {
                returnValue[i] = System.Convert.ToByte(temp[i]);
            }

            return returnValue;
        }
        public static string GetGlobalSettingValue(string key)
        {
            return _GlobalSettingKeyValue[key];
        }
        public static short[] GetShortArrayGSV(string key)
        {
            return GetShortArray(_GlobalSettingKeyValue[key]);
        }
        public static byte[] GetByteArrayGSV(string key)
        {
            return GetByteArray(_GlobalSettingKeyValue[key]);
        }

        public static ushort[] GetUshortArrayGSV(string key)
        {
            return GetUshortArray(_GlobalSettingKeyValue[key]);
        }
        public static float GetFloatGSV(string key)
        {
            return float.Parse(_GlobalSettingKeyValue[key]);
        }
        public static ushort GetUshortGSV(string key)
        {
            return ushort.Parse(_GlobalSettingKeyValue[key]);
        }
    }
}