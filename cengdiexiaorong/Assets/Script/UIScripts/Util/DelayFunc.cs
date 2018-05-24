using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public delegate void ObjectsFun(object[] param);
public delegate void VoidFun();
public delegate void EventDelegate(GameObject obj, PointerEventData eventData);
public struct SDelayFun
{
	public ObjectsFun	ev;
	public float	fDoTime;
	public object[] param;
}

public class DelayFunc 
{
    public delegate void Callback();
    public delegate void Callback<T>(T arg1);
    public delegate void Callback<T, U>(T arg1, U arg2);
    public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);

    public static void CallFuncDelay(Callback fun, float fDelayTime)
    {
        Task.CreateTask(DoFunc(fun, fDelayTime));
    }
    public static IEnumerator DoFunc(Callback fun, float fDelayTime)
    {
        if (fun == null)
            yield break;

        yield return new WaitForSeconds(fDelayTime);
        fun();
    }

    public static void CallFuncDelay<T>(Callback<T> fun, float fDelayTime, T param)
    {
        Task.CreateTask(DoFunc(fun, fDelayTime, param));
    }
    public  static IEnumerator DoFunc<T>(Callback<T> fun, float fDelayTime, T param)
    {
        if (fun == null)
            yield break;

        yield return new WaitForSeconds(fDelayTime);
        fun(param);
    }

    public static void CallFuncDelay<T, U>(Callback<T, U> fun, float fDelayTime, T param, U param2)
    {
        Task.CreateTask(DoFunc(fun, fDelayTime, param, param2));
    }
    public static IEnumerator DoFunc<T, U>(Callback<T, U> fun, float fDelayTime, T param1, U param2)
    {
        if (fun == null)
            yield break;

        yield return new WaitForSeconds(fDelayTime);
        fun(param1, param2);
    }

    public static void CallFuncDelay<T, U, V>(Callback<T, U, V> fun, float fDelayTime, T param, U param2, V param3)
    {
        Task.CreateTask(DoFunc(fun, fDelayTime, param, param2, param3));
    }
    public static IEnumerator DoFunc<T, U, V>(Callback<T, U, V> fun, float fDelayTime, T param1, U param2, V param3)
    {
        if (fun == null)
            yield break;

        yield return new WaitForSeconds(fDelayTime);
        fun(param1, param2, param3);
    }

}
