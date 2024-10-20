using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories;
using Talabat.Repository;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

            Services.AddAutoMapper(typeof(MappingProfiles));

            #region Error Handling
            Services.Configure<ApiBehaviorOptions>(options =>
                {
                    options.InvalidModelStateResponseFactory = (actionContext) =>
                    {
                        // ModelState => Dic [KeyValuePair]
                        // Key => Name of parameter
                        // Value => Error

                        var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                     .SelectMany(P => P.Value.Errors)
                                     .Select(E => E.ErrorMessage)
                                     .ToArray();

                        var ValidationErrorResponse = new ApiValidationErrorResponse()
                        {
                            Errors = errors
                        };
                        return new BadRequestObjectResult(ValidationErrorResponse);

                    };
                }); 
            #endregion

            return Services;
        }

    }
}
