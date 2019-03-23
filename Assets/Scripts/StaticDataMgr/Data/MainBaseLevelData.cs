using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StaticData.Data
{
    public class MainBaseLevelData : BaseDataObject
    {
        
		public byte Level = 0;	//等级等级
		public string MainPrefabName = "";	//主城Prefab名称
		public byte MainbaseType = 0;	//类型
		public byte Radius = 0;	//领地半径
		public int GrainLimit = 0;	//粮食上限
		public int GoldLimit = 0;	//金矿上限
		public int SoldierLimit = 0;	//屯兵上限
		public int UpLevelGrainRequire = 0;	//升级粮食需求
		public int UpLevelGoldRequire = 0;	//升级金矿需求
		
        public override void ReadFromStream(BinaryReader br)
        {
            mID = br.ReadUInt16();	//ID
			Level = br.ReadByte();	//等级等级
			MainPrefabName = br.ReadString();	//主城Prefab名称
			MainbaseType = br.ReadByte();	//类型
			Radius = br.ReadByte();	//领地半径
			GrainLimit = br.ReadInt32();	//粮食上限
			GoldLimit = br.ReadInt32();	//金矿上限
			SoldierLimit = br.ReadInt32();	//屯兵上限
			UpLevelGrainRequire = br.ReadInt32();	//升级粮食需求
			UpLevelGoldRequire = br.ReadInt32();	//升级金矿需求
			
        }
    } 
} 