using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RG
{
    public class Delay : MonoBehaviour
    {
        public float WaitTime;
        public float completionTime;

        public Delay(float waitTime)
        {
            WaitTime = waitTime;
            Reset();
        }

        public void Reset()
        {
            completionTime = Time.time + WaitTime;
        }

        public bool IsReady
        {
            get { return Time.time >= completionTime; }
        }
    }
}
