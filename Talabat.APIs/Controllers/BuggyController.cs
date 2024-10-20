﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : APIBaseController
    {
        private readonly StoreContext _dbContext;

        public BuggyController(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("NotFound")]
        // baseUrl/api/Buggy/NotFound
        public ActionResult GetNotFoundRequest ()
        {
            var Product = _dbContext.Products.Find(100);

            if (Product is null) return NotFound(new ApiResponse(404));

            return Ok(Product);

        }

        [HttpGet("ServerError")]
        // baseUrl/api/Buggy/ServerError
        public ActionResult GetServerError()
        {
            var Product = _dbContext.Products.Find(100);
            var ProductToReturn = Product.ToString(); // Error
            // Will Throw Exception [Null Reference Exception]
            return Ok(ProductToReturn);

        }

        [HttpGet("BadRequest")]
        public ActionResult GetbadRequest()
        {
            return BadRequest();
        }

        [HttpGet("BadRequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }


    }
}
