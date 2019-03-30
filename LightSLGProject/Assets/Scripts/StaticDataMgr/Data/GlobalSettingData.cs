using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StaticData.Data
{
    public class GlobalSettingData : BaseDataObject
    {
        
		public string Name = "";	//属性名称
		public string PropValue = "";	//属性值
		
        public override void ReadFromStream(BinaryReader br)
        {
            mID = br.ReadUInt16();	//ID
			Name = br.ReadString();	//属性名称
			PropValue = br.ReadString();	//属性值
			
        }
    } 
} 