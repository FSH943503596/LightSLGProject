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

    private byte _Id;                                               //玩家ID
    private byte _GroupID;                                          //分组ID
    private string _Name;                                           //玩家名称
    private IList<MainBaseVO> _MainBases = new List<MainBaseVO>();  //玩家拥有的主基地

    public PlayerVO() { }
    public PlayerVO(string name, byte groupID)
    {
        this._Name = name;
        this._GroupID = groupID;
        _Id = OriginID++;
    }

    public IList<MainBaseVO> MainBases { get => _MainBases; }
    public byte Id { get => _Id;}

    /// <summary>
    /// 增加基地
    /// </summary>
    /// <param name="mainBase"></param>
    /// <returns>添加的基地是否设置成主基地</returns>
    public bool AddMainBases(MainBaseVO mainBase) {
        if (mainBase == null) return false;

        _MainBases.Add(mainBase);

        return _MainBases.Count < 2;
    }

    public bool IsUser => _Id == 0;

    public void RemoveMainBaseVO(MainBaseVO vO)
    {
        if (vO == null || !_MainBases.Contains(vO)) return;

        _MainBases.Remove(vO);
    }
}

