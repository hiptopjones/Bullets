using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    // https://blog.oliverbooth.dev/2021/04/27/how-do-unitys-coroutines-actually-work/
    internal class WaitForTime : IEnumerator
    {
        // TODO: This should use TimeManager to get the current time, so that time scale is respected
        private readonly DateTime _endTime;

        public WaitForTime(TimeSpan waitTime)
        {
            _endTime = DateTime.Now + waitTime;
        }

        public bool MoveNext() => DateTime.Now < _endTime;

        public void Reset() { }

        public object Current => null;
    }
}
