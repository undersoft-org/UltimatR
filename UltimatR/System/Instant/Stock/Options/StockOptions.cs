using System.Instant;
using System.Runtime.InteropServices;

namespace System.Instant.Stock
{
    public class StockOptions<T> : StockOptions
    {
        public StockOptions() : base(typeof(T))
        {
        }
        public StockOptions(IFigures figures)
            : base(figures)
        {
        }
    }

    public class StockOptions
    {
        public StockOptions() { }

        public StockOptions(IFigures figures) : this(figures.Type, figures.FigureSize)
        {
            this.figures = figures;
        }
        public StockOptions(Type type)
        {
            Type = type;
            blocksize = Marshal.SizeOf(type);
        }
        public StockOptions(Type type, int blockSize)
        {
            Type = type;
            blocksize = blockSize;
        }

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        protected string stockname;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        protected string basepath;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        protected string stockpath;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        protected string sectorsuffix;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        protected string regitrysuffix;
        protected ushort sectorsize = 50 * 1000;
        protected ushort clustersize = 20;
        protected int    blocksize = 0;
        protected bool   oversized = false;
        protected bool   isowner = true;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        protected string typestring;
        protected Type type;
        protected IFigures figures;

        public IFigures Figures
        {
            get => figures;
            set => figures = value;
        }

        public Type Type
        {
            get => type ??= typestring == null ? 
                type = Type.GetType(typestring = typeof(byte[]).FullName) : 
                type = Type.GetType(typestring);
            set 
            { 
                typestring = value.FullName;
                type = value;
            }
        }

        public virtual string RegistrySuffix
        {
            get => regitrysuffix ??= "str";
            set => regitrysuffix = value;
        }

        public virtual string SectorSuffix
        {
            get => sectorsuffix ??= "std";
            set => sectorsuffix = value;
        }

        public virtual string FileName => $"{type.Name}.{SectorSuffix}";

        public virtual string StockName => $"{BasePath}__{type.FullName}.{SectorSuffix}";

        public virtual string FilePath => $"{BasePath}/{type.Name}/{FileName}";

        public virtual string StockPath => $"{BasePath}/{type.Name}";

        public virtual string BasePath
        {
            get => basepath ??= "SBFS";
            set => basepath = value;
        }

        public virtual ushort SectorSize
        {
            get => sectorsize;
            set => sectorsize = value;
        }

        public virtual ushort ClusterSize
        {
            get => clustersize;
            set => clustersize = value;
        }

        public virtual bool Oversized
        {
            get => oversized;
            set => oversized = value;
        }

        public virtual bool IsOwner
        {
            get => isowner;
            set => isowner = value;
        }

        public virtual int BlockSize
        {
            get => blocksize;
            set => blocksize = value;
        }
    }
}