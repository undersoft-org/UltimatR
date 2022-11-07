using System.Linq;
using System.Linq.Expressions;

namespace UltimatR
{
    internal class TeleRepositoryExpressionVisitor : ExpressionVisitor
    {
        #region Fields

        private readonly IQueryable newRoot;

        #endregion

        #region Constructors

        public TeleRepositoryExpressionVisitor(IQueryable newRoot)
        {
            this.newRoot = newRoot;
        }

        #endregion

        #region Methods

        protected override Expression VisitConstant(ConstantExpression node) =>
             node.Type.BaseType != null && 
             node.Type.BaseType.IsGenericType && 
             node.Type.BaseType.GetGenericTypeDefinition() == typeof(TeleRepository<>) ? 
             Expression.Constant(newRoot) : node;

        #endregion
    }
}
