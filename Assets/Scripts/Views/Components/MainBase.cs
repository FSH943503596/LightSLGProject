/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-25-17:15:15
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/
using UnityEngine;

public class MainBase : Building
{
    private MainBaseVO m_Data;
    public Transform m_EdgeF;
    public Transform m_EdgeR;
    public Transform m_EdgeB;
    public Transform m_EdgeL;

    public void InitMainBase(MainBaseVO vO)
    {
        this.m_Data = vO;
    }
    public void UpdateArea()
    {
        if (m_EdgeF && m_EdgeR && m_EdgeB && m_EdgeL)
        {
            Vector3 newPosition = new Vector3(m_EdgeF.childCount + 1, m_EdgeF.childCount + 1, 0);
            m_EdgeF.transform.localPosition += Vector3.forward;
            Transform child = m_EdgeF.GetChild(m_EdgeF.childCount + 1);
            Instantiate<Transform>(child).localPosition = newPosition;

            m_EdgeB.transform.localPosition -= Vector3.forward;
            child = m_EdgeF.GetChild(m_EdgeF.childCount + 1);
            Instantiate<Transform>(child).localPosition = newPosition;

            m_EdgeR.transform.localPosition += Vector3.left;
            child = m_EdgeF.GetChild(m_EdgeR.childCount + 1);
            Instantiate<Transform>(child).localPosition = newPosition;

            m_EdgeL.transform.localPosition -= Vector3.left;
            child = m_EdgeF.GetChild(m_EdgeL.childCount + 1);
            Instantiate<Transform>(child).localPosition = newPosition;
        }
    }
}