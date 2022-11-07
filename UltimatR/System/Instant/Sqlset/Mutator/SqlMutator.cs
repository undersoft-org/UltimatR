
// <copyright file="SqlMutator.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Sqlset namespace.
/// </summary>
namespace System.Instant.Sqlset
{
    using System.Series;




    /// <summary>
    /// Class SqlMutator.
    /// </summary>
    public class SqlMutator
    {
        #region Fields

        /// <summary>
        /// The sqaf
        /// </summary>
        private Sqlbase sqaf;

        #endregion

        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="SqlMutator" /> class.
        /// </summary>
        public SqlMutator()
        {
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="SqlMutator" /> class.
        /// </summary>
        /// <param name="insql">The insql.</param>
        public SqlMutator(Sqlbase insql)
        {
            sqaf = insql;
        }

        #endregion

        #region Methods







        /// <summary>
        /// Deletes the specified SQL connect string.
        /// </summary>
        /// <param name="SqlConnectString">The SQL connect string.</param>
        /// <param name="cards">The cards.</param>
        /// <returns>IDeck&lt;IDeck&lt;IFigure&gt;&gt;.</returns>
        public IDeck<IDeck<IFigure>> Delete(string SqlConnectString, IFigures cards)
        {
            try
            {
                if (sqaf == null)
                    sqaf = new Sqlbase(SqlConnectString);
                try
                {
                    bool buildmap = true;
                    if (cards.Count > 0)
                    {
                        BulkPrepareType prepareType = BulkPrepareType.Drop;
                        return sqaf.Delete(cards, true, buildmap, prepareType);
                    }
                    return null;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }








        /// <summary>
        /// Sets the specified SQL connect string.
        /// </summary>
        /// <param name="SqlConnectString">The SQL connect string.</param>
        /// <param name="cards">The cards.</param>
        /// <param name="Renew">if set to <c>true</c> [renew].</param>
        /// <returns>IDeck&lt;IDeck&lt;IFigure&gt;&gt;.</returns>
        public IDeck<IDeck<IFigure>> Set(string SqlConnectString, IFigures cards, bool Renew)
        {
            try
            {
                if (sqaf == null)
                    sqaf = new Sqlbase(SqlConnectString);
                try
                {
                    bool buildmap = true;
                    if (cards.Count > 0)
                    {
                        BulkPrepareType prepareType = BulkPrepareType.Drop;

                        if (Renew)
                            prepareType = BulkPrepareType.Trunc;

                        var ds = sqaf.Update(cards, true, buildmap, true, null, prepareType);
                        if (ds != null)
                        {
                            IFigures im = (IFigures)Summon.New(cards.GetType());
                            im.Rubrics = cards.Rubrics;
                            im.FigureType = cards.FigureType;
                            im.FigureSize = cards.FigureSize;
                            im.Add(ds["Failed"].AsValues());
                            return sqaf.Insert(im, true, false, prepareType);
                        }
                        else
                            return null;
                    }
                    return null;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
