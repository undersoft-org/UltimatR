
// <copyright file="Laborator.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

/// <summary>
/// The Laboring namespace.
/// </summary>
namespace System.Laboring
{
    using System.Collections;
    using System.Series;
    using System.Threading;

    /// <summary>
    /// Class Laborator.
    /// Implements the <see cref="System.Laboring.ILaborator" />
    /// </summary>
    /// <seealso cref="System.Laboring.ILaborator" />
    public class Laborator : ILaborator
    {
        #region Fields

        /// <summary>
        /// The wait write timeout
        /// </summary>
        private static readonly int WAIT_WRITE_TIMEOUT = 5000;

        /// <summary>
        /// The post access
        /// </summary>
        private ManualResetEventSlim postAccess = new ManualResetEventSlim(true, 128);
        /// <summary>
        /// The post pass
        /// </summary>
        private SemaphoreSlim postPass = new SemaphoreSlim(1);
        /// <summary>
        /// The inlock
        /// </summary>
        private object inlock = new object();
        /// <summary>
        /// The outlock
        /// </summary>
        private object outlock = new object();

        /// <summary>
        /// Acquires the post access.
        /// </summary>
        private void acquirePostAccess()
        {
            do
            {
                if (!postAccess.Wait(WAIT_WRITE_TIMEOUT))
                    continue;     
                postAccess.Reset();
            }
            while (!postPass.Wait(0));
        }
        /// <summary>
        /// Releases the post access.
        /// </summary>
        private void releasePostAccess()
        {
            postPass.Release();
            postAccess.Set();
        }

        /// <summary>
        /// The notes
        /// </summary>
        public LaborNotes Notes;
        /// <summary>
        /// The ready
        /// </summary>
        public bool Ready;
        /// <summary>
        /// The case
        /// </summary>
        public LaborCase Case;
        /// <summary>
        /// The aspect
        /// </summary>
        public Aspect Aspect;
        /// <summary>
        /// The laborers
        /// </summary>
        private Thread[] laborers;
        /// <summary>
        /// Gets the laborers count.
        /// </summary>
        /// <value>The laborers count.</value>
        private int LaborersCount => Aspect.LaborersCount;
        /// <summary>
        /// The elaborations
        /// </summary>
        private Catalog<Laborer> Elaborations = new Catalog<Laborer>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Laborator" /> class.
        /// </summary>
        /// <param name="aspect">The aspect.</param>
        public Laborator(Aspect aspect)
        {
            Aspect = aspect;
            Case = Aspect.Case;
            Notes = Case.Notes;
            Ready = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Closes the specified safe close.
        /// </summary>
        /// <param name="SafeClose">if set to <c>true</c> [safe close].</param>
        public void Close(bool SafeClose)
        {
            foreach (Thread laborer in laborers)
            {
                Run(null);

                if (SafeClose && laborer.ThreadState == ThreadState.Running)
                    laborer.Join();
            }
            Ready = false;
        }

        /// <summary>
        /// Allocates the specified antcount.
        /// </summary>
        /// <param name="laborersCount">The laborers count.</param>
        /// <returns>Aspect.</returns>
        public Aspect Allocate(int laborersCount = 0)
        {
            if (laborersCount > 0)
                Aspect.LaborersCount = laborersCount;

            laborers = new Thread[LaborersCount];
            for (int i = 0; i < LaborersCount; i++)
            {
                laborers[i] = new Thread(Activate);
                laborers[i].IsBackground = true;
                laborers[i].Priority = ThreadPriority.AboveNormal;
                laborers[i].Start();
            }

            Ready = true;
            return Aspect;
        }

        /// <summary>
        /// Runs the specified labor.
        /// </summary>
        /// <param name="work">The work.</param>
        public void Run(Labor work)
        {
            lock (inlock)
            {
                if (work != null)
                {
                    Elaborations.Enqueue(Clone(work.Laborer));
                    Monitor.Pulse(inlock);

                }
                else
                {
                    Elaborations.Enqueue(DateTime.Now.Ticks, null);
                    Monitor.Pulse(inlock);
                }
            }
        }

        /// <summary>
        /// Resets the specified antcount.
        /// </summary>
        /// <param name="laborersCount">The laborers count.</param>
        public void Reset(int laborersCount = 0)
        {
            Close(true);
            Allocate(laborersCount);
        }

        /// <summary>
        /// Activates this instance.
        /// </summary>
        public void Activate()
        {
            for (; ; )
            {
                Laborer laborer = null;
                object input = null;

                lock (inlock)
                {
                    while (!Elaborations.TryDequeue(out laborer))
                    {
                        Monitor.Wait(inlock);
                    }

                    if (laborer != null) input = laborer.GetInput();
                }

                if (laborer == null) return;

                object output;
                if (input != null)
                {
                    if (input is IList)
                        output = laborer.Process.Execute((object[])input);
                    else
                        output = laborer.Process.Execute(input);
                }
                else
                {
                    output = laborer.Process.Execute();
                }

                lock (outlock)
                {
                    Outpost(laborer, output);
                }                
            }
        }

        /// <summary>
        /// Clones the specified laborer.
        /// </summary>
        /// <param name="laborer">The laborer.</param>
        /// <returns>Laborer.</returns>
        private Laborer Clone(Laborer laborer)
        {
            Laborer _laborer = new Laborer(laborer.Name, laborer.Process);
            _laborer.SetInput(laborer.GetInput());
            _laborer.Evokers = laborer.Evokers;
            _laborer.Labor = laborer.Labor;
            return _laborer;
        }

        /// <summary>
        /// Outposts the specified worker.
        /// </summary>
        /// <param name="worker">The worker.</param>
        /// <param name="output">The output.</param>
        private void Outpost(Laborer worker, object output)
        {
            if (output != null)
            {
                worker.SetOutput(output);

                if (worker.Evokers != null && worker.Evokers.Count > 0)
                {
                    int l = worker.Evokers.Count;
                    if (l > 0)
                    {
                        var notes = new Note[l];
                        for (int i = 0; i < worker.Evokers.Count; i++)
                        {
                            Note note = new Note(worker.Labor, worker.Evokers[i].Recipient, worker.Evokers[i], null, output);
                            note.SenderBox = worker.Labor.Box;
                            notes[i] = note;
                        }

                        Notes.Send(notes);
                    }
                }

            }
        }

        #endregion
    }
}
