using System.Net;
using FluentValidation.Results;
using InforceTestTask.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace InforceTestTask.Domain.Extensions;

public static class ControllerExtensions
{
    public static IActionResult HandleResponse(
        this ControllerBase controller, 
        ApiResponse apiResponse)
    {
        if (apiResponse.IsSuccess)
        {
            return controller.Ok(new { message = apiResponse.Description });
        }

        if (apiResponse is { Description: ValidationResult validationResult, Status: HttpStatusCode.BadRequest })
        {
            var errors = validationResult.ToDictionary();
            return controller.BadRequest(new { errors });
        }

        return CreateResponse(controller, apiResponse.Status, apiResponse.Description);
    }

    public static IActionResult HandleResponse<T>(
        this ControllerBase controller, 
        ApiResponse<T> apiResponse)
    {
        if (apiResponse.IsSuccess)
        {
            return controller.Ok(new { data = apiResponse.Data });
        }

        if (apiResponse is { Error: ValidationResult validationResult, Status: HttpStatusCode.BadRequest })
        {
            var errors = validationResult.ToDictionary();
            return controller.BadRequest(new { errors });
        }

        return CreateResponse(controller, apiResponse.Status, apiResponse.Error);
    }
    
    private static IActionResult CreateResponse(
        this ControllerBase controller,
        HttpStatusCode statusCode,
        object? error)
    {
        var errorObject = error switch
        {
            null => new { message = "Unexpected error occurred during the request." },
            string str when string.IsNullOrWhiteSpace(str) => new { message = "Unexpected error occurred during the request." },
            string str => new { message = str },
            _ => error
        };

        return statusCode switch
        {
            HttpStatusCode.NotFound => controller.NotFound(errorObject),
            HttpStatusCode.Unauthorized => controller.Unauthorized(),
            HttpStatusCode.Conflict => controller.Conflict(errorObject),
            HttpStatusCode.Forbidden => controller.StatusCode((int)HttpStatusCode.Forbidden, errorObject),
            HttpStatusCode.InternalServerError => controller.StatusCode((int)HttpStatusCode.InternalServerError, errorObject),
            _ => controller.BadRequest(errorObject),
        };
    }
}