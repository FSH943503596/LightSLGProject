using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StaticData.Data
{
    public class AbilityData : BaseDataObject
    {
        
		public byte Level = 0;	//当前等级
		public ushort NextID = 0;	//下一级ID
		public byte AbilityType = 0;	//技能类型 
		public ushort Name = 0;	//技能名称 
		public ushort Description = 0;	//技能描述
		public string ICON = "";	//技能图标
		public uint PowerValue = 0;	//技能战斗力（绝对值）
		public ushort Fury = 0;	//怒气获取
		public ushort UnlockCost = 0;	//解锁消耗(技能需求)
		public uint CoolTime = 0;	//技能CD
		public byte CostType = 0;	//消耗资源类型
		public ushort Cost = 0;	//资源消耗值
		public List<byte> ActionAnimation = new List<byte>();	//技能动作
		public uint AnimationTime = 0;	//动画持续时间
		public List<ushort> EffectList = new List<ushort>();	//Effect列表
		public List<uint> EffectDelay = new List<uint>();	//Effect延迟
		public List<string> EffectResource = new List<string>();	//特效 
		public List<uint> DelayTime = new List<uint>();	//特效延迟
		public float[] EffectResourceScale = new float[3];	//特效大小
		public string ReleaseAudio = "";	//施法音效
		public ushort AddDelay = 0;	//技能硬直
		
        public override void ReadFromStream(BinaryReader br)
        {
            mID = br.ReadUInt16();	//ID
			Level = br.ReadByte();	//当前等级
			NextID = br.ReadUInt16();	//下一级ID
			AbilityType = br.ReadByte();	//技能类型 
			Name = br.ReadUInt16();	//技能名称 
			Description = br.ReadUInt16();	//技能描述
			ICON = br.ReadString();	//技能图标
			PowerValue = br.ReadUInt32();	//技能战斗力（绝对值）
			Fury = br.ReadUInt16();	//怒气获取
			UnlockCost = br.ReadUInt16();	//解锁消耗(技能需求)
			CoolTime = br.ReadUInt32();	//技能CD
			CostType = br.ReadByte();	//消耗资源类型
			Cost = br.ReadUInt16();	//资源消耗值
			ushort listCount_13 = br.ReadUInt16();
			for(ushort i = 0; i < listCount_13; i++)
				ActionAnimation.Add(br.ReadByte());	//技能动作
			AnimationTime = br.ReadUInt32();	//动画持续时间
			ushort listCount_15 = br.ReadUInt16();
			for(ushort i = 0; i < listCount_15; i++)
				EffectList.Add(br.ReadUInt16());	//Effect列表
			ushort listCount_16 = br.ReadUInt16();
			for(ushort i = 0; i < listCount_16; i++)
				EffectDelay.Add(br.ReadUInt32());	//Effect延迟
			ushort listCount_17 = br.ReadUInt16();
			for(ushort i = 0; i < listCount_17; i++)
				EffectResource.Add(br.ReadString());	//特效 
			ushort listCount_18 = br.ReadUInt16();
			for(ushort i = 0; i < listCount_18; i++)
				DelayTime.Add(br.ReadUInt32());	//特效延迟
			ushort cnt3_19 = br.ReadUInt16();
			for(ushort i = 0; i < cnt3_19; i++)
				EffectResourceScale[i] = br.ReadSingle();	//特效大小
			ReleaseAudio = br.ReadString();	//施法音效
			AddDelay = br.ReadUInt16();	//技能硬直
			
        }
    } 
} 