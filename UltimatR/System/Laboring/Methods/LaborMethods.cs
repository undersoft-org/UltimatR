namespace System.Laboring
{
    using System.Series;




    public class LaborMethods : Catalog<IDeputy>
    {
        #region Methods






        public override ICard<IDeputy>[] EmptyDeck(int size)
        {
            return new LaborMethod[size];
        }





        public override ICard<IDeputy> EmptyCard()
        {
            return new LaborMethod();
        }






        public override ICard<IDeputy>[] EmptyCardTable(int size)
        {
            return new LaborMethod[size];
        }






        public override ICard<IDeputy> NewCard(IDeputy value)
        {
            return new LaborMethod(value);
        }







        public override ICard<IDeputy> NewCard(object key, IDeputy value)
        {
            return new LaborMethod(key, value);
        }







        public override ICard<IDeputy> NewCard(ulong key, IDeputy value)
        {
            return new LaborMethod(key, value);
        }

        #endregion
    }
}
