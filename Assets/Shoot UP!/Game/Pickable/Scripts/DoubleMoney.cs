using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game_.PickableObjects
{
    public class DoubleMoney : PickableObject
    {
        public float duration => this._duration;
        [SerializeField] private float _duration;
    }

}
