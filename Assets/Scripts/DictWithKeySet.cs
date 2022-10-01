using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictWithKeySet<TKey, TValue> : Dictionary<TKey, TValue>
{

    public TKey[] GetKeys()
    {
        TKey[] ret = new TKey[this.Count];

        this.Keys.CopyTo(ret, 0);

        return ret;
    }
}