using System.Collections;
using UnityEngine;

namespace Game_.PickableObjects
{
    public class Shield : PickableObject
    {
        public float duration => this._duration;
        [SerializeField] private float _duration;
    }
}