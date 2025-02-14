using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class DependenciesCollection
{
    private Dictionary<Type, Dependency> dependencies = new Dictionary<Type, Dependency>();
    private Dictionary<Type, object> singletons = new Dictionary<Type, object>();

    public void Add(Dependency dependency) => dependencies.Add(dependency.Type, dependency);

    public object Get(Type type)
    {
        if (dependencies.ContainsKey(type) is false)
        {
            throw new ArgumentException($"{type.FullName} dependency type not found in DependenciesCollection.dependencies");
        }

        // If a singleton is being called, instantiate it if it's not been created yet, then return the singleton to the caller.
        var dependency = dependencies[type];
        if (dependency.IsSingleton is true)
        {
            if (singletons.ContainsKey(type) is false)
            {
                singletons.Add(type, dependency);
            }
            return singletons[type];
        }
        else
        {
            // If not a singleton, have the factory create an instance of the requested dependency and return that dependency.
            return dependency.Factory();
        }
    }

    // Generic call to Get.
    public T Get<T>()
    {
        return (T)Get(typeof(T));
    }
}