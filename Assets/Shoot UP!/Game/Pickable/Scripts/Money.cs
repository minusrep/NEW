using System.Collections;
using UnityEngine;

namespace Game_.PickableObjects
{
    public class Money : PickableObject
    {
        public int amount => this._amount;

        [SerializeField] private int _amount;
    }
}