using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StoreManagement.Utils
{
    public static class ControllerBaseExtension
    {
        /// <summary>
        ///  Creates an Object Result that produces a Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError response.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.ObjectResult with Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError for the response.</returns>
        public static ObjectResult InternalServerError(this ControllerBase controller, object? value)
        {
            return controller.StatusCode(StatusCodes.Status500InternalServerError, value);
        }
    }
}