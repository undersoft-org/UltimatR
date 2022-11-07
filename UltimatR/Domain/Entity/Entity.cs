using System.ComponentModel.DataAnnotations.Schema;

namespace UltimatR
{
    using System;
    using System.ComponentModel;
    using System.Instant;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using System.Uniques;

    [StructLayout(LayoutKind.Sequential)]
    public class Entity : Identifiable, IEntity, INotifyPropertyChanged
    {
        #region Fields

        private IRepository repository;
        IRepository IEntity.Repository => repository;

        //private Variety variety;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors

        public Entity()
        {
            Initialize();
        }

        #endregion

        #region Properties

        Ussn IFigure.SerialCode { get => serialcode; set => serialcode = value; }

        [NotMapped]
        object[] IFigure.ValueArray { get => sleeve.ValueArray; set => sleeve.ValueArray = value; }
        
        #endregion

        #region Methods

        private void Initialize()
        {
            var type = this.ProxyRetype();     
            
            if (type.IsAssignableTo(typeof(ISleeve)) ||
                type.IsAssignableTo(typeof(Event)))
                return;

            createValueProxy(type);

            attachRepository(type);
        }

        private void createValueProxy(Type type)
        {
            var s = SleeveFactory.Create(type, (uint)this.UniqueSeed);
            sleeve = s.Combine(this);
            //variety = new Variety(type);
            //variety.Combine(sleeve);
        }

        private void attachRepository(Type type) 
        {
            repository = ThreadSpot.Get<IRepository>();
            ThreadSpot.AddTyped(type, this);
        }

        #endregion
    }
}
