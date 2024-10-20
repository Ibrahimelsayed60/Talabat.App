using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
		// Function to Build Query
		// // _dbContext.Products.Where(P => P.Id == id).Include(P => P.ProductBrand).Include(P => P.ProductType)

		public static IQueryable<T> GetQuery (IQueryable<T> inputQuery, ISpecifications<T> Spec)
        {
            var Query = inputQuery; // _dbContext.Products

            if(Spec.Criteria is not null) // P => P.Id == id
			{
                Query = Query.Where(Spec.Criteria); // _dbContext.Products.Where(P => P.Id == id)
			}

			// P => P.ProductBrand, P => P.ProductType
			Query = Spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));


			return Query;
		}
    }
}
