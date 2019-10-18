using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int type;
    public bool hitboxEnabled;

    public Block(int _type, bool _hitboxEnabled = true){
        type = _type;
        hitboxEnabled = _hitboxEnabled;
    }
}
