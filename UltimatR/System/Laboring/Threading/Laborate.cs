
namespace System.Laboring
{
    using System.Linq;
    using System.Series;

    public class Laborate
    {
        #region Fields

        public Aspect Aspect;
        public Case Case;
        public Labor Labor;
        public Laborator Laborator;

        #endregion

        #region Constructors

        public Laborate(bool safeClose, string className, string methodName, out object result, params object[] input)
            : this(1, safeClose, Summon.New(className), methodName, input)
        {
            result = Labor.GetOutput();
        }
        public Laborate(IDeputy method)
            : this(1, false, method)
        {
        }
        public Laborate(IDeputy method, bool safe, params object[] input)
            : this(1, safe, method, input)
        {
        }
        public Laborate(IDeputy method, params object[] input)
            : this(1, false, method, input)
        {
        }
        public Laborate(int laborersCount, bool safeClose, IDeck<IDeputy> _methods)
        {
            Case = new Case();
            Aspect = Case.Aspect("FirstLaborNow");
            foreach(var am in _methods)
                Aspect.Include(am);
            Aspect.Allocate(laborersCount);

            Laborator = Aspect.Laborator;
            foreach(Labor am in Aspect)
                am.Run(am.ParameterValues);

            Aspect.Laborator.Close(safeClose);
        }
        public Laborate(int laborersCount, bool safeClose, IDeputy method, params object[] input)
        {
            Case = new Case();
            Aspect = Case.Aspect("FirstLaborNow")
                .Include(method)
                .Aspect.Allocate(laborersCount);

            Laborator = Aspect.Laborator;
            Labor = Aspect.AsValues().ElementAt(0);
            Case.Run(method.Name, input);
            Laborator.Close(safeClose);
        }
        public Laborate(int laborersCount, bool safeClose, object classObject, string methodName, params object[] input)
        {
            IDeputy am = new Deputy(classObject, methodName);
            Case = new Case();
            Aspect = Case.Aspect("FirstLaborNow")
                .Include(am)
                .Aspect.Allocate(laborersCount);

            Laborator = Aspect.Laborator;
            Labor = Aspect.AsValues().ElementAt(0);
            Case.Run(am.Name, input);
            Laborator.Close(safeClose);
        }
        public Laborate(int laborersCount, int evokerCount, bool safeClose, IDeputy method, IDeputy evoker)
        {
            Case = new Case();
            Aspect = Case.Aspect("FirstLaborNow")
                .Include(method)
                .Aspect.Allocate(laborersCount);
            Case.Aspect("SecondLaborNow")
                .Include(evoker)
                .Aspect.Allocate(evokerCount);

            Laborator = Aspect.Laborator;
            Labor = Aspect.AsValues().ElementAt(0);
            Labor.ResultTo(Case.AsValues().Skip(1).FirstOrDefault().AsValues().FirstOrDefault());
            Case.Run(method.Name, method.ParameterValues);
            Laborator.Close(safeClose);
        }
        public Laborate(object classObject, string methodName, out object result, params object[] input)
            : this(1, false, classObject, methodName, input)
        {
            result = Labor.GetOutput();
        }
        public Laborate(string className, string methodName, params object[] input)
            : this(1, false, Summon.New(className), methodName, input)
        {
        }

        #endregion

        #region Methods

        public static Laborate Run<TClass>()
        {
            return new Laborate(new Deputy<TClass>());
        }
        public static Laborate Run<TClass>(bool safeThread, params object[] input)
        {
            return new Laborate(new Deputy<TClass>(), safeThread, input);
        }
        public static Laborate Run<TClass>(object[] constructorParams, params object[] input)
        {
            return new Laborate(new Deputy<TClass>(constructorParams), input);
        }
        public static Laborate Run<TClass>(string methodName, object[] constructorParams, params object[] input)
        {
            return new Laborate(new Deputy<TClass>(methodName, constructorParams), input);
        }
        public static Laborate Run<TClass>(string methodName, params object[] input)
        {
            return new Laborate(new Deputy<TClass>(methodName), input);
        }
        public static Laborate Run<TClass>(string methodName, Type[] parameterTypes, object[] constructorParams, params object[] input)
        {
            return new Laborate(new Deputy<TClass>(methodName, parameterTypes, constructorParams), input);
        }
        public static Laborate Run<TClass>(string methodName, Type[] parameterTypes, params object[] input)
        {
            return new Laborate(new Deputy<TClass>(methodName, parameterTypes), input);
        }
        public static Laborate Run<TClass>(Type[] parameterTypes, object[] constructorParams, params object[] input)
        {
            return new Laborate(new Deputy<TClass>(parameterTypes, constructorParams), input);
        }
        public static Laborate Run<TClass>(Type[] parameterTypes, params object[] input)
        {
            return new Laborate(new Deputy<TClass>(parameterTypes), input);
        }

        public void Close(bool safeClose = false)
        {
            Aspect.Laborator.Close(safeClose);
        }

        public void Run()
        {
            Laborator.Run(Labor);
        }
        public void Run(params object[] input)
        {
            this.Labor.SetInput(input);
            Laborator.Run(this.Labor);
        }

        #endregion
    }
}
