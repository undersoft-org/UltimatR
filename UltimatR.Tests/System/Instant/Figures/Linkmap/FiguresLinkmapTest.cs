/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Tests.FiguresLinkmapTest.cs.Tests
   
   @project: Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Tests
{
    using System.Instant.Linking;
    using System.Linq;
    using System.Series;
    using System.Uniques;
    using Xunit;

    public class FiguresLinkmapTest
    {
        #region Methods

        //[Fact]
        //public void FiguresLinkmap_Test()
        //{
        //    IFigures figuresA = new Figures<FieldsAndPropertiesModel>("Figures_A_Test", FigureMode.Reference).Combine();

        //    IFigures figuresB = new Figures<FieldsAndPropertiesModel>("Figures_B_Test", FigureMode.Reference).Combine();

        //    figuresA =  FiguresLinkmap_AddFigures_A_Helper_Test(figuresA);

        //    figuresB =  FiguresLinkmap_AddFigures_B_Helper_Test(figuresB);

        //    //Link fl = new Link(figuresA, figuresB, figuresA.KeyRubrics);

        //    //NodeCatalog nc = Linker.Map;
        //    //LinkMember olm = fl.Origin;
        //    //LinkMember tlm = fl.Target;
        //    //var ocards = figuresA.AsCards().ToArray();
            //var tcards = figuresB.AsCards().ToArray();



            //for (int i = 0; i < ocards.Length; i++)
            //{
            //    var ocard = ocards[i];
            //    var tcard = tcards[i];
            //    ulong olinkKey = olm.LinkKey(ocard.Value);
            //    ulong tlinkKey = tlm.LinkKey(tcard.Value);

                //if (!nc.TryGet(olinkKey, out ICard<IFigure> obranch))
                //    nc.Add(new BranchDeck(fl, ocard).FirstOrDefault());
                //else
                //    obranch.Deck.Put(ocard);

                //if (!nc.TryGet(tlinkKey, out ICard<IFigure> tbranch))
                //    nc.Add(new BranchDeck(fl, tcard).FirstOrDefault());
                //else
                //    tbranch.Deck.Put(tcard);
           // }
        //}

        private IFigures FiguresLinkmap_AddFigures_A_Helper_Test(IFigures figures)
        {
            IFigures _figures = figures;
            FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
            IFigure figureMock = _figures.NewFigure();//FiguresLinkmap_SetValues_Helper_Test(_figures, fom);
            int idSeed = (int)figureMock["Id"];
            DateTime seedKeyTick = DateTime.Now;
            for (int i = 0; i < 100000; i++)
            {
                IFigure figure = _figures.NewFigure();
                figure.ValueArray = figureMock.ValueArray;
                figure[7] = new Ussn(30000 + i * 300);
                figure.UniqueKey = (ulong)(int.MaxValue - (i * 30));
                figure.UniqueSeed = (ulong)(30000 + i * 300);
                _figures.Put(figure);
            }
            return _figures;
        }

        private IFigures FiguresLinkmap_AddFigures_B_Helper_Test(IFigures figures)
        {
            IFigures _figures = figures;
            FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
            IFigure figureMock = _figures.NewFigure(); //FiguresLinkmap_SetValues_Helper_Test(_figures, fom);
            int idSeed = (int)figureMock["Id"];
            DateTime seedKeyTick = DateTime.Now;
            for (int i = 0; i < 100000; i++)
            {
                {
                    IFigure figure = _figures.NewFigure();
                    figure.ValueArray = figureMock.ValueArray;
                    figure[7] = new Ussn(30000 + i * 300);
                    figure.UniqueKey = (ulong)(int.MaxValue + (i * 30));
                    figure.UniqueSeed = (ulong)(30000 + i * 300);
                    _figures.Put(figure);
                }
            }
            return _figures;
        }

        #endregion
    }
}
