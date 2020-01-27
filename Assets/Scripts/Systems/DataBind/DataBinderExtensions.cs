using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DataBinderExtensions
{
    public static TGet Bind<T, TGet>(this MonoBehaviour mono, TGet dependent = null)
        where T : class, IDataBind<TGet> 
        where TGet : class
    {
        var rootObjs = mono.gameObject.scene.GetRootGameObjects();
        var dependentList = new List<IDataBind<TGet>>();

        foreach (var rootObj in rootObjs)
        {
            var rootDependencies = rootObj.GetComponentsInChildren<T>(includeInactive: true);
            dependentList.AddRange(rootDependencies);
        }

        if (dependent == null)
        {
            foreach (var rootObj in rootObjs)
            {
                dependent = rootObj.GetComponentInChildren<TGet>(includeInactive: true);

                if (dependent != null)
                    break;
            }
        }

        if (dependent == null && dependentList.Count > 0)
        {
            Debug.LogError($"Could not bind {typeof(TGet)} from {mono.name}", mono);
            return null;
        }

        var initializer = dependent as IInitialize;
        initializer?.Init();

        foreach (var dependency in dependentList)
        {
            if (dependency is MonoBehaviour dMono)
            {
                try
                {
                    var dependentInParent = dMono.GetComponentsInParent<TGet>(includeInactive: true)
                        .First(x => (x as MonoBehaviour)?.gameObject != dMono.gameObject);

                    dependent = dependentInParent ?? dependent;
                }
                catch
                {
                }
            }

            dependency.Bind(dependent);
        }

        return dependent;
    }
}