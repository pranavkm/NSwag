﻿//-----------------------------------------------------------------------
// <copyright file="OperationResponseProcessor.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/NSwag/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NJsonSchema;
using NJsonSchema.Infrastructure;
using NSwag.SwaggerGeneration.Processors;
using NSwag.SwaggerGeneration.Processors.Contexts;
using NSwag.SwaggerGeneration.WebApi.Processors;

namespace NSwag.SwaggerGeneration.AspNetCore
{
    /// <summary>Generates the operation's response objects based on reflection and the ResponseTypeAttribute, SwaggerResponseAttribute and ProducesResponseTypeAttribute attributes.</summary>
    public class OperationResponseProcessor : IOperationProcessor
    {
        private readonly AspNetCoreToSwaggerGeneratorSettings _settings;

        /// <summary>Initializes a new instance of the <see cref="OperationParameterProcessor"/> class.</summary>
        /// <param name="settings">The settings.</param>
        public OperationResponseProcessor(AspNetCoreToSwaggerGeneratorSettings settings)
        {
            _settings = settings;
        }

        /// <summary>Processes the specified method information.</summary>
        /// <param name="operationProcessorContext"></param>
        /// <returns>true if the operation should be added to the Swagger specification.</returns>
        public async Task<bool> ProcessAsync(OperationProcessorContext operationProcessorContext)
        {
            if (!(operationProcessorContext is AspNetCoreOperationProcessorContext context))
                return false;

            var parameter = context.MethodInfo.ReturnParameter;
            var successXmlDescription = await parameter.GetDescriptionAsync(parameter.GetCustomAttributes()).ConfigureAwait(false) ?? string.Empty;

            var responseTypeAttributes = context.MethodInfo.GetCustomAttributes()
                .Where(a => a.GetType().Name == "ResponseTypeAttribute" ||
                            a.GetType().Name == "SwaggerResponseAttribute")
                .Concat(context.MethodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes()
                    .Where(a => a.GetType().Name == "SwaggerResponseAttribute"))
                .ToList();

            var operation = context.OperationDescription.Operation;
            foreach (var requestFormat in context.ApiDescription.SupportedRequestFormats)
            {
                if (!operation.Consumes.Contains(requestFormat.MediaType, StringComparer.OrdinalIgnoreCase))
                {
                    operation.Consumes.Add(requestFormat.MediaType);
                }
            }

            if (responseTypeAttributes.Count > 0)
            {
                // if SwaggerResponseAttribute \ ResponseTypeAttributes are present, we'll only use those.
                var builder = new SwaggerResponseBuilder(context, _settings, "200", successXmlDescription);
                builder.PopulateModelsFromResponseTypeAttributes(responseTypeAttributes);

                await builder.BuildSwaggerResponseAsync(parameter);
            }
            else
            {
                foreach (var apiResponse in context.ApiDescription.SupportedResponseTypes)
                {
                    var returnType = apiResponse.Type;
                    var response = new SwaggerResponse();
                    string httpStatusCode;
                    if (apiResponse.StatusCode == 0 && IsVoidResponse(returnType))
                        httpStatusCode = "200";
                    else if (apiResponse.TryGetPropertyValue<bool>("IsDefaultResponse"))
                        httpStatusCode = "default";
                    else
                        httpStatusCode = apiResponse.StatusCode.ToString(CultureInfo.InvariantCulture);

                    var typeDescription = _settings.ReflectionService.GetDescription(
                        returnType, context.MethodInfo.ReturnParameter?.GetCustomAttributes(), _settings);

                    if (IsVoidResponse(returnType) == false)
                    {
                        response.IsNullableRaw = typeDescription.IsNullable;

                        response.Schema = await context.SchemaGenerator
                            .GenerateWithReferenceAndNullability<JsonSchema4>(
                                returnType, null, typeDescription.IsNullable, context.SchemaResolver)
                            .ConfigureAwait(false);
                    }

                    context.OperationDescription.Operation.Responses[httpStatusCode] = response;

                    foreach (var responseFormat in apiResponse.ApiResponseFormats)
                    {
                        if (!context.Document.Produces.Contains(responseFormat.MediaType, StringComparer.OrdinalIgnoreCase))
                        {
                            context.Document.Produces.Add(responseFormat.MediaType);
                        }
                    }
                }
            }

            return true;
        }

        private bool IsVoidResponse(Type returnType)
        {
            return returnType == null || returnType.FullName == "System.Void";
        }
    }
}