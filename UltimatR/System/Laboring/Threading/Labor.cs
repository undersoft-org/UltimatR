
namespace System.Laboring
{
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Uniques;

    public class Labor<T> : Labor
    {
        public Labor(Func<T, string> method) : base(new Deputy<T>(method)) { }
    }

    public class Labor : Task<object>, IDeputy, ILaborer
    {
        public IUnique Empty => Ussn.Empty;

        public Labor(IDeputy operation) : base(() => operation.Execute())
        {
            Name = operation.Name;
            Laborer = new Laborer(operation.Name, operation);
            Laborer.Labor = this;
            Box = new NoteBox(Laborer.Name);
            Box.Labor = this;
            
            SerialCode = new Ussn(Name.UniqueKey(), Unique.New);
        }
        public Labor(Laborer laborer) : base(() => laborer.Process.Execute())
        {
            Name = laborer.Name;
            Laborer = laborer;
            Laborer.Labor = this;
            Box = new NoteBox(Laborer.Name);
            Box.Labor = this;

            SerialCode = new Ussn(Name.UniqueKey(), Unique.New);
        }

        public string Name { get; set; }

        public string QualifiedName { get; set; }

        public Laborer Laborer { get; set; }

        public Aspect Aspect { get; set; }

        public LaborCase Case { get; set; }

        public NoteBox Box { get; set; }

        public object[] ParameterValues
        {
            get => Laborer.Process.ParameterValues;
            set => Laborer.Process.ParameterValues = value;
        }
        public object this[int fieldId] { get => ParameterValues[fieldId]; set => ParameterValues[fieldId] = value; }
        public object this[string propertyName]
        {
            get
            {
                for (int i = 0; i < Parameters.Length; i++)
                    if (Parameters[i].Name == propertyName)
                        return ParameterValues[i];
                return null;
            }
            set
            {
                for (int i = 0; i < Parameters.Length; i++)
                    if (Parameters[i].Name == propertyName)
                        ParameterValues[i] = value;
            }
        }

        public MethodInfo Info
        {
            get => Laborer.Process.Info;
            set => Laborer.Process.Info = value;
        }

        public ParameterInfo[] Parameters
        {
            get => Laborer.Process.Parameters;
            set => Laborer.Process.Parameters = value;
        }
        public object[] ValueArray { get => ParameterValues; set => ParameterValues = value; }

        public Ussn SerialCode
        {
            get;
            set;
        }

        public ulong UniqueKey { get => SerialCode.UniqueKey; set => SerialCode.SetUniqueKey(value); }

        public ulong UniqueSeed { get => SerialCode.UniqueSeed; set => SerialCode.SetUniqueSeed(value); }

        public MethodDeputy MethodDeputy => Process.MethodDeputy;

        public Delegate Method => Process.Method;

        public void Run(params object[] input)
        {
            Laborer.SetInput(input);
            Aspect.Run(this);
        }

        public object Execute(params object[] parameters)
        {
            this.Run(parameters);
            return null;
        }

        public byte[] GetBytes()
        {
            return Laborer.Process.GetBytes();
        }
        public byte[] GetUniqueBytes()
        {
            return SerialCode.GetUniqueBytes();
        }

        public bool Equals(IUnique other)
        {
            return SerialCode.Equals(other);
        }
        public int CompareTo(IUnique other)
        {
            return SerialCode.CompareTo(other);
        }

        public Task<object> ExecuteAsync(params object[] parameters)
        {
            return Task.Run(() => { return Execute(parameters); });
        }

        public Task<T> ExecuteAsync<T>(params object[] parameters)
        {
            return Task.Run(() => { Execute(parameters); return default(T); });
        }

        public NoteEvokers Evokers
        {
            get => Laborer.Evokers;
            set => Laborer.Evokers = value;
        }

        public string LaborerName
        {
            get => Laborer.Name;
            set => Laborer.Name = value;
        }

        public IDeputy Process
        {
            get => Laborer.Process;
            set => Laborer.Process = value;
        }
        public object TargetObject { get => Process.TargetObject; set => Process.TargetObject=value; }

        public void ResultTo(Labor Recipient)
        {
            ulong recipientKey = Recipient.Name.UniqueKey();

            var relationLabors = Aspect.AsValues()
                .Where(l => l.Laborer.Evokers
                        .ContainsKey(l.Name.UniqueKey(recipientKey)))
                .ToArray();

            var evokers = relationLabors.Select(l => l.Evokers.Get(l.Name.UniqueKey(recipientKey))).ToArray();
            foreach(var noteEvoker in evokers)
            {
                noteEvoker.RelatedLabors.Put(this);
                noteEvoker.RelatedLaborNames.Put(Name);
            }

            Laborer.ResultTo(Recipient, relationLabors.Concat(new[] { this }).ToArray());
        }
        public void ResultTo(Labor Recipient, params Labor[] RelationLabors)
        {
            Laborer.ResultTo(Recipient, RelationLabors);
        }
        public void ResultTo(string RecipientName)
        {
            var recipient = Case.AsValues()
                .Where(m => m.ContainsKey(RecipientName))
                .SelectMany(os => os.AsValues()).FirstOrDefault();

            ulong recipientKey = RecipientName.UniqueKey();

            var relationLabors = Aspect.AsValues()
                .Where(l => l.Laborer.Evokers
                    .ContainsKey(l.Name.UniqueKey(recipientKey))).ToArray();

            var evokers = relationLabors.Select(l => l.Evokers.Get(l.Name.UniqueKey(recipientKey))).ToArray();
            foreach(var noteEvoker in evokers)
            {
                noteEvoker.RelatedLabors.Put(this);
                noteEvoker.RelatedLaborNames.Put(Name);
            }

            Laborer.ResultTo(recipient, relationLabors.Concat(new[] { this }).ToArray());
        }
        public void ResultTo(string RecipientName, params string[] RelationNames)
        {
            Laborer.ResultTo(RecipientName, RelationNames);
        }

        public virtual Aspect Include<T>() where T : class
        {
            return Aspect.Include<T>();
        }
        public virtual Aspect Include<T>(Type[] arguments) where T : class
        {
            return Aspect.Include<T>(arguments);
        }
        public virtual Aspect Include<T>(params object[] consrtuctorParams) where T : class
        {
            return Aspect.Include<T>(consrtuctorParams);
        }
        public virtual Aspect Include<T>(Type[] arguments, params object[] consrtuctorParams) where T : class
        {
            return Aspect.Include<T>(arguments, consrtuctorParams);
        }

        public virtual Aspect Include<T>(Func<T, string> method) where T : class
        {
            return Aspect.Include<T>(method);
        }
        public virtual Aspect Include<T>(Func<T, string> method, params Type[] arguments) where T : class
        {
            return Aspect.Include<T>(method, arguments);
        }
        public virtual Aspect Include<T>(Func<T, string> method, params object[] constructorParams) where T : class
        {
            return Aspect.Include<T>(method, constructorParams);
        }

        public virtual Labor Operation<T>() where T : class
        {
            return Aspect.Operation<T>();
        }
        public virtual Labor Operation<T>(Type[] arguments) where T : class
        {
            return Aspect.Operation<T>(arguments);
        }
        public virtual Labor Operation<T>(params object[] consrtuctorParams) where T : class
        {
            return Aspect.Operation<T>(consrtuctorParams);
        }
        public virtual Labor Operation<T>(Type[] arguments, params object[] consrtuctorParams) where T : class
        {
            return Aspect.Operation<T>(arguments, consrtuctorParams);
        }

        public virtual Labor Operation<T>(Func<T, string> method) where T : class
        {
            return Aspect.Operation<T>(method);
        }
        public virtual Labor Operation<T>(Func<T, string> method, params Type[] arguments) where T : class
        {
            return Aspect.Operation<T>(method, arguments);
        }
        public virtual Labor Operation<T>(Func<T, string> method, params object[] constructorParams) where T : class
{
            return Aspect.Operation<T>(method, constructorParams);
        }

        public Aspect Allocate(int laborsCount = 1)
        {
            return Aspect.Allocate(laborsCount);
        }

        public object GetInput()
        {
            return ((ILaborer)Laborer).GetInput();
        }

        public void SetInput(object value)
        {
            ((ILaborer)Laborer).SetInput(value);
        }

        public object GetOutput()
        {
            return ((ILaborer)Laborer).GetOutput();
        }

        public void SetOutput(object value)
        {
            ((ILaborer)Laborer).SetOutput(value);
        }

        public Task Publish(params object[] parameters)
        {
            return Process.Publish(parameters);
        }

        public object Execute(object target, params object[] parameters)
        {
            return Process.Execute(target, parameters);
        }

        public Task<object> ExecuteAsync(object target, params object[] parameters)
        {
            return Process.ExecuteAsync(target, parameters);
        }
        public Task<T> ExecuteAsync<T>(object target, params object[] parameters)
        {
            return Process.ExecuteAsync<T>(target, parameters);
        }

        public Task Publish(bool firstAsTarget, object target, params object[] parameters)
        {
            return Process.Publish(firstAsTarget, target, parameters);
        }
        public object Execute(bool firstAsTarget, object target, params object[] parameters)
        {
            return Process.Execute(firstAsTarget, target, parameters);
        }

        public Task<object> ExecuteAsync(bool firstAsTarget, object target, params object[] parameters)
        {
            return Process.ExecuteAsync(firstAsTarget, target, parameters);
        }
        public Task<T> ExecuteAsync<T>(bool firstAsTarget, object target, params object[] parameters)
        {
            return Process.ExecuteAsync<T>(firstAsTarget, target, parameters);
        }
    }
}
