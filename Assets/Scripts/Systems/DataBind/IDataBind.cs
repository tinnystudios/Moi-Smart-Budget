using UnityEngine;

/// <summary>
/// An interface for requiring a particular data
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDataBind<T>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    void Bind(T data);
}
