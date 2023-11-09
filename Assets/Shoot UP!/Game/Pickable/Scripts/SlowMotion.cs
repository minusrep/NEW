using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Game_.PickableObjects
{
    public class SlowMotion : PickableObject
    {
        public float duration => this._duration;
        public float slowdown => this._slowdown;
        [SerializeField] private float _duration;
        [SerializeField][Range(0f, 1f)] private float _slowdown;
    }

}
