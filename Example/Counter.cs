using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Example
{
    public static class Counter
    {
        private static int _publishCount = 0;
        private static int _consumeCount = 0;

        public static int GetPublishCount
        {
            get
            {
                return (int)_publishCount;
            }
        }

        public static int IncrementPublish()
        {
            Interlocked.Increment(ref _publishCount);
            return (int)_publishCount;
        }

        public static int IncrementConsume()
        {
            Interlocked.Increment(ref _consumeCount);
            return (int)_consumeCount;
        }

        public static int DecrementPublish()
        {
            Interlocked.Decrement(ref _publishCount);
            return (int)_publishCount;
        }
        public static int DecrementConsume()
        {
            Interlocked.Decrement(ref _consumeCount);
            return (int)_consumeCount;
        }


    }
}
