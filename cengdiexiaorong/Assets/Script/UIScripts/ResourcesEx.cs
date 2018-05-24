using UnityEngine;
using System.Collections;

public class ResourcesEx 
{
    public static event System.Action OnAysncCountChange; 
    static int m_asyncLoadCount = 0;
    public static int AsyncLoadCount {
        set{
            m_asyncLoadCount = value; 
            //Debug.LogWarning(m_asyncLoadCount);

            if (OnAysncCountChange != null)
            {
                OnAysncCountChange();
            }
        } 
        get{return m_asyncLoadCount;}
    }

    public static Object[] LoadAll(string szName) {
        if (AsyncLoadCount != 0)
        {
            Develop.LogError("AsyncLoad res, can't ResourcesEx.Load" + szName);
            return null;
        }

        return Resources.LoadAll(szName);
    }

    public static IEnumerator IEAsyncLoad<T>(string szName, System.Action<T> onLoaded) where T : Object
    {
        //
        while (AsyncLoadCount != 0)
        {
            yield return null;
        }
        T obj = Resources.Load<T>(szName) as T;
        if(onLoaded != null)
        {
            onLoaded(obj);
        }
        yield return obj;
    }

    public static T Load<T>(string szName) where T : Object
    {
        if (AsyncLoadCount != 0)
        {
            Develop.LogError("AsyncLoad res, can't ResourcesEx.Load" + szName);
            return default(T);
        }

        return Resources.Load<T>(szName);    
    }
}
