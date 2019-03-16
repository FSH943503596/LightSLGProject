/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-14-15:30:07
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

public interface IBattleManager
{
    /// <summary>
    /// 战斗是否结束
    /// </summary>
    bool IsBattleOver { get; set; }
    /// <summary>
    /// 战斗初始化
    /// </summary>
    void Initinal();
    /// <summary>
    /// 战斗更新
    /// </summary>
    void Update();
    /// <summary>
    /// 战斗结束，释放资源
    /// </summary>
    void Release();
}

