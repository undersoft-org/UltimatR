using System.Linq;
using System.Linq.Expressions;

namespace UltimatR
{
    internal class HostRepositoryExpressionVisitor : ExpressionVisitor
    {
        #region Fields

        private readonly IQueryable newRoot;

        #endregion

        #region Constructors

        public HostRepositoryExpressionVisitor(IQueryable newRoot)
        {
            this.newRoot = newRoot;
        }

        #endregion

        #region Methods

        protected override Expression VisitConstant(ConstantExpression node) =>
             node.Type.BaseType != null && 
             node.Type.BaseType.IsGenericType && 
             node.Type.BaseType.GetGenericTypeDefinition() == typeof(HostRepository<>) ? 
             Expression.Constant(newRoot) : node;

        #endregion
    }
}
