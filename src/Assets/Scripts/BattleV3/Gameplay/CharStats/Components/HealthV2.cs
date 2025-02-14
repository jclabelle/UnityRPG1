using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthV2 : StatV2
{
    public override int Decrease(int value)
    {
        base.Decrease(value);

        return value;
    }
}
