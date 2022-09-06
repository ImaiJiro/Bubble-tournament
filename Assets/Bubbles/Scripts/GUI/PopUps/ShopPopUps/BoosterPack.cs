using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
    [CreateAssetMenu]
    public class BoosterPack : ScriptableObject
    {
        public BoosterFunc boosterFunc;
        public int count;
    }
}
