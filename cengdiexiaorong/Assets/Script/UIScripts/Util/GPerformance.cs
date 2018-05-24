using UnityEngine;
using System.Collections;
using System;

//性能收集工具
public class GPerformance
{
    public static bool IfPerformance = true;
    private static UnityEngine.UI.ObjectPool<GPerformance> s_pool;         // pool
    private static GPerformance m_tmp = null;

    private DateTime m_timeStart = DateTime.MinValue;
    private string m_strTag = "";

    //开始收集
    public static GPerformance Start(string strTag)
    {
        if(!IfPerformance)
        {
            if(m_tmp == null)
            {
                m_tmp = new GPerformance();
            }
            return m_tmp;
        }

        if(s_pool == null)
        {
            s_pool = new UnityEngine.UI.ObjectPool<GPerformance>((obj)=> { }, (obj)=> { });
        }

        GPerformance pfm = (GPerformance)s_pool.Get();
        pfm.m_timeStart = DateTime.Now;
        pfm.m_strTag = strTag;
        return pfm;
    }

    //过程中的时间输出
    public void LogPoint(int iPoint = 999)
    {
        if (!IfPerformance)
        {
            return;
        }

        //如果花销小于1ms的 不打印日志
        double dInterval = (DateTime.Now - m_timeStart).TotalMilliseconds;
        if (dInterval > 10)
        {
            Debug.LogFormat("<color=#73FA3EFF>【GPerformance】【{0}】--{1}-----cost time:{2}ms</color>", m_strTag, iPoint, dInterval);
        }
        else if (dInterval >= 1)
        {
            Debug.LogFormat("【GPerformance】【{0}】--{1}-----cost time:{2}ms", m_strTag, iPoint, dInterval);
        }
    }
	
    public void Stop(bool bRelease = true)
    {
        if(!IfPerformance)
        {
            return;
        }

        LogPoint();

        s_pool.Release(this);
    }
}
