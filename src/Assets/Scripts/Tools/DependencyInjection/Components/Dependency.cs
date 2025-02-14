using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class Dependency
{
    public Type Type { get; set; }
    public Func<object> Factory { get; set; }
    public bool IsSingleton { get; set; }
}