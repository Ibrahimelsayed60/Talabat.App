using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
	public interface ISpecifications<T> where T : BaseEntity
	{
        // _dbContext.Products.Where(P => P.Id == id).Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync()

        // Sign of Property for Where Condition [P => P.Id == id]
        public Expression<Func<T, bool>> Criteria { get; set; }

        // Sign of Property For List of Includes [Include(P => P.ProductBrand).Include(P => P.ProductType)]
        public List<Expression<Func<T, object>>> Includes { get; set; }

        // Prop for orderBy [OrderBy(P => P.Name)]
        public Expression<Func<T,object>> orderBy { get; set; }

        // Prop for orderByDesc [OrderByDesc(P => P.Name)]
        public Expression<Func<T, object>> orderByDescending { get; set; }

        // TAKE(2)
        public int Take { get; set; }

        // SKIP(2)
        public int Skip { get; set; }

        public bool IsPaginationEnabled { get; set; }

    }
}
