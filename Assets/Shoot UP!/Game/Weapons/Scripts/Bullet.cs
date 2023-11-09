using System.Collections;
using UnityEngine;

namespace Game_.Weapons
{
    public class Bullet : MonoBehaviour
    {

        public void ApplyMovement(float speed)
        {
            this.transform.Translate(Vector3.right * Time.deltaTime);
        }
    }
}