using System;
using System.Instant;
using System.Runtime.InteropServices;
using System.Uniques;

namespace UltimatR
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class Dto : Identifiable, IDto
    {
        public Dto()
        {
            Initialize();
        }

        object[] IFigure.ValueArray
        {
            get => sleeve.ValueArray;
            set => sleeve.ValueArray = value;
        }

        Ussn IFigure.SerialCode
        {
            get => serialcode;
            set => serialcode = value;
        }

        private void Initialize()
        {
            var type = this.ProxyRetype();

            if (type.IsAssignableTo(typeof(ISleeve)) ||
                type.IsAssignableTo(typeof(Event)))
                return;

            createValueProxy(type);
        }

        private void createValueProxy(Type type)
        {
            var s = SleeveFactory.Create(type, (uint)this.UniqueSeed);
            sleeve = s.Combine(this);
        }
    }
}   