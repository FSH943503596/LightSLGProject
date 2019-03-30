/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-25-11:51:02
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;
using System.Collections.Generic;

public class PlayerVO
{
    private static byte OriginID = 0;

    private byte _Id;                                                   //玩家ID
    private byte _GroupID;                                              //分组ID
    private string _Name;                                               //玩家名称
    private int _ColorIndex;                                            //玩家颜色编号 0：蓝, 1:青, 2:灰, 3:绿, 4:红, 5:黄
    private IList<MainBaseVO> _MainBases = new List<MainBaseVO>();      //玩家拥有的主基地
    private int _Gold = 100;                                            //玩家金矿数量
    private int _GoldLimit = 1000;                                      //玩家金矿上下
    private int _Grain = 100;                                           //玩家粮食数量
    private int _GrainLimit = 1000;                                     //玩家粮食数量上限
    private int _SoldierAmount = 0;                                     //玩家士兵数量
    private int _SoldierAmountLimit = 0;                                //玩家士兵数量上限

    public PlayerVO() { }
    public PlayerVO(string name, byte groupID)
    {
        this._Name = name;
        this._GroupID = groupID;
        _Id = OriginID++;
    }

    public IList<MainBaseVO> mainBases { get => _MainBases; }
    public byte Id { get => _Id;}

    /// <summary>
    /// 增加基地
    /// </summary>
    /// <param name="mainBase"></param>
    /// <returns>添加的基地是否设置成主基地</returns>
    public bool AddMainBases(MainBaseVO mainBase) {
        if (mainBase == null || _MainBases.Contains(mainBase)) return false;

        _MainBases.Add(mainBase);

        _GoldLimit += mainBase.goldLimit;
        _GrainLimit += mainBase.grainLimit;
        _SoldierAmountLimit += mainBase.soldierNumLimit;

        return _MainBases.Count < 2;
    }

    public bool IsUser => _Id == 0;

    public int gold { get => _Gold; set => _Gold = value; }
    public int goldLimit { get => _GoldLimit; set => _GoldLimit = value; }
    public int grain { get => _Grain; set => _Grain = value; }
    public int grainLimit { get => _GrainLimit; set => _GrainLimit = value; }
    public int soldierAmount { get => _SoldierAmount; set => _SoldierAmount = value; }
    public int soldierAmountLimit { get => _SoldierAmountLimit; set => _SoldierAmountLimit = value; }
    public int colorIndex { get => _ColorIndex; set => _ColorIndex = value; }

    public bool isSoldierBelowLimit => _SoldierAmountLimit > _SoldierAmount;
    public bool isGrainBelowLimit => _GrainLimit > _Grain;
    public bool isGoldBelowLimit => _GoldLimit > _Gold;
    public void RemoveMainBaseVO(MainBaseVO vO)
    {
        if (vO == null || !_MainBases.Contains(vO)) return;

        //去除所有上限加成

        _GoldLimit -= vO.goldLimit;
        _GrainLimit -= vO.grainLimit;
        _SoldierAmountLimit -= vO.soldierNumLimit;

        _MainBases.Remove(vO);
    }
}

