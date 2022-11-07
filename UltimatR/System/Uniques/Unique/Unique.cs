/// <summary>
/// The Uniques namespace.
/// </summary>
namespace System.Uniques
{
    using System.Collections.Concurrent;
    using System.Threading;

    /// <summary>
    /// Class Unique.
    /// </summary>
    public static class Unique
    {
        /// <summary>
        /// The capacity
        /// </summary>
        private static readonly int CAPACITY = 75 * 1000;
        /// <summary>
        /// The low limit
        /// </summary>
        private static readonly int LOW_LIMIT = 50 * 1000;
        /// <summary>
        /// The next key vector
        /// </summary>
        private static readonly uint NEXT_KEY_VECTOR = (uint)PRIMES_ARRAY.Get(4);
        /// <summary>
        /// The wait loops
        /// </summary>
        private static readonly int WAIT_LOOPS = 500;
        /// <summary>
        /// The bit32
        /// </summary>
        private static Unique32 bit32 = new Unique32();
        /// <summary>
        /// The bit64
        /// </summary>
        private static Unique64 bit64 = new Unique64();
        /// <summary>
        /// The generating
        /// </summary>
        private static bool generating;
        /// <summary>
        /// The generator
        /// </summary>
        private static Thread generator;
        /// <summary>
        /// The holder
        /// </summary>
        private static object holder = new object();
        /// <summary>
        /// The key number
        /// </summary>
        private static ulong keyNumber = (ulong)DateTime.Now.Ticks;
        /// <summary>
        /// The keys
        /// </summary>
        private static ConcurrentQueue<ulong> keys = new ConcurrentQueue<ulong>();
        /// <summary>
        /// The random seed
        /// </summary>
        private static Random randomSeed = new Random((int)(DateTime.Now.Ticks.UniqueKey32()));

        /// <summary>
        /// Initializes static members of the <see cref="Unique" /> class.
        /// </summary>
        static Unique()
        {
            generator = startup();
        }

        /// <summary>
        /// Gets the bit32.
        /// </summary>
        /// <value>The bit32.</value>
        public static Unique32 Bit32 { get => bit32; }

        /// <summary>
        /// Gets the bit64.
        /// </summary>
        /// <value>The bit64.</value>
        public static Unique64 Bit64 { get => bit64; }

        /// <summary>
        /// Gets the new.
        /// </summary>
        /// <value>The new.</value>
        public static ulong New
        {
            get
            {
                ulong key = 0;
                int counter = 0;
                bool loop = false;
                while (counter < WAIT_LOOPS)
                {
                    if (!(loop = keys.TryDequeue(out key)))
                    {
                        if (!generating)
                            Start();

                        counter++;
                        Thread.Sleep(20);
                    }
                    else
                    {
                        int count = keys.Count;
                        if (count < LOW_LIMIT)
                            Start();
                        break;
                    }
                }
                return key;
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public static void Start()
        {
            lock (holder)
            {
                if (!generating)
                {
                    generating = true;
                    Monitor.Pulse(holder);
                }
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public static void Stop()
        {
            if (generating)
            {
                generating = false;
            }
        }

        /// <summary>
        /// Keys the generation.
        /// </summary>
        private unsafe static void keyGeneration()
        {
            while(generating)
            {
                lock(holder)
                {
                    ulong seed = nextSeed();
                    int count = CAPACITY - keys.Count;
                    for(int i = 0; i < count; i++)
                    {
                        ulong keyNo = nextKeyNumber();
                        keys.Enqueue(Hasher64.ComputeKey(((byte*)&keyNo), 8, seed));
                    }
                    Stop();
                    Monitor.Wait(holder);
                }
            }
        }

        /// <summary>
        /// Nexts the key number.
        /// </summary>
        /// <returns>System.UInt64.</returns>
        private static unsafe ulong nextKeyNumber()
        {
            return keyNumber += NEXT_KEY_VECTOR;
        }

        /// <summary>
        /// Nexts the seed.
        /// </summary>
        /// <returns>System.UInt64.</returns>
        private static ulong nextSeed()
        {
            return (ulong)randomSeed.Next();
        }

        /// <summary>
        /// Startups this instance.
        /// </summary>
        /// <returns>Thread.</returns>
        private static Thread startup()
        {
            generating = true;
            Thread _reffiler = new Thread(new ThreadStart(keyGeneration));
            _reffiler.Start();
            return _reffiler;
        }

    }
}
