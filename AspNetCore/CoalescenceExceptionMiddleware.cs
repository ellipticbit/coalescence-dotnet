using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using EllipticBit.Coalescence.AspNetCore;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace EllipticBit.Coalescence.AspNetCore
{
	public class CoalescenceExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly JsonSerializerOptions _options;

		/// <summary>
		/// Initializes a new instance of the <see cref="CoalescenceExceptionMiddleware"/> class.
		/// </summary>
		/// <param name="next">The next middleware in the pipeline.</param>
		public CoalescenceExceptionMiddleware(RequestDelegate next, IOptions<JsonOptions> jsonOptionsAccessor) {
			_next = next;
			_options = jsonOptionsAccessor.Value.SerializerOptions;
		}

		/// <summary>
		/// Invokes the middleware.
		/// </summary>
		/// <param name="context">The HTTP context.</param>
		/// <returns>A task that represents the completion of request processing.</returns>
		public async Task InvokeAsync(HttpContext context) {
			try {
				await _next(context);
			} catch (CoalescenceHttpException ex) {
				context.Response.StatusCode = await ex.GetResponseData(context.Response.BodyWriter, _options);
			}
		}
	}

	public abstract class CoalescenceHttpException : Exception
	{
		private readonly int statusCode = 0;
		private readonly object data = null;

		protected CoalescenceHttpException(int statusCode) {
			this.statusCode = statusCode;
		}

		protected CoalescenceHttpException(string message, int statusCode) : base(message) {
			this.statusCode = statusCode;
		}

		protected CoalescenceHttpException(object data, int statusCode) {
			this.statusCode = statusCode;
			this.data = data;
		}

		internal async Task<int> GetResponseData(System.IO.Pipelines.PipeWriter writer, JsonSerializerOptions options) {
			if (data == null && !string.IsNullOrWhiteSpace(Message)) {
				await writer.WriteAsync(Encoding.UTF8.GetBytes(Message));
			}
			else if (data != null) {
				await writer.WriteAsync(JsonSerializer.SerializeToUtf8Bytes(data, options));
			}

			return statusCode;
		}
	}

	public sealed class HttpBadRequestException : CoalescenceHttpException
	{
		public HttpBadRequestException() : base(StatusCodes.Status400BadRequest) { }
		public HttpBadRequestException(string message) : base(message, StatusCodes.Status400BadRequest) { }
		public HttpBadRequestException(object data) : base(data, StatusCodes.Status400BadRequest) { }
	}

	public sealed class HttpUnauthorizedException : CoalescenceHttpException
	{
		public HttpUnauthorizedException() : base(StatusCodes.Status401Unauthorized) { }
		public HttpUnauthorizedException(string message) : base(message, StatusCodes.Status401Unauthorized) { }
	}

	public sealed class HttpForbiddenException : CoalescenceHttpException
	{
		public HttpForbiddenException() : base(StatusCodes.Status403Forbidden) { }
		public HttpForbiddenException(string message) : base(message, StatusCodes.Status403Forbidden) { }
	}

	public sealed class HttpNotFoundException : CoalescenceHttpException
	{
		public HttpNotFoundException() : base(StatusCodes.Status404NotFound) { }
		public HttpNotFoundException(string message) : base(message, StatusCodes.Status404NotFound) { }
		public HttpNotFoundException(object data) : base(data, StatusCodes.Status404NotFound) { }
	}

	public sealed class HttpConflictException : CoalescenceHttpException
	{
		public HttpConflictException() : base(StatusCodes.Status409Conflict) { }
		public HttpConflictException(string message) : base(message, StatusCodes.Status409Conflict) { }
		public HttpConflictException(object data) : base(data, StatusCodes.Status409Conflict) { }
	}

	public sealed class HttpGoneException : CoalescenceHttpException
	{
		public HttpGoneException() : base(StatusCodes.Status410Gone) { }
		public HttpGoneException(string message) : base(message, StatusCodes.Status410Gone) { }
	}

	public sealed class HttpLengthException : CoalescenceHttpException
	{
		public HttpLengthException() : base(StatusCodes.Status411LengthRequired) { }
		public HttpLengthException(string message) : base(message, StatusCodes.Status411LengthRequired) { }
	}

	public sealed class HttpPreconditionFailedException : CoalescenceHttpException
	{
		public HttpPreconditionFailedException() : base(StatusCodes.Status412PreconditionFailed) { }
		public HttpPreconditionFailedException(string message) : base(message, StatusCodes.Status412PreconditionFailed) { }
		public HttpPreconditionFailedException(object data) : base(data, StatusCodes.Status412PreconditionFailed) { }
	}

	public sealed class HttpPayloadSizeException : CoalescenceHttpException
	{
		public HttpPayloadSizeException() : base(StatusCodes.Status413PayloadTooLarge) { }
		public HttpPayloadSizeException(string message) : base(message, StatusCodes.Status413PayloadTooLarge) { }
	}

	public sealed class HttpUnsupportedException : CoalescenceHttpException
	{
		public HttpUnsupportedException() : base(StatusCodes.Status415UnsupportedMediaType) { }
		public HttpUnsupportedException(string message) : base(message, StatusCodes.Status415UnsupportedMediaType) { }
	}

	public sealed class HttpExpectationFailedException : CoalescenceHttpException
	{
		public HttpExpectationFailedException() : base(StatusCodes.Status417ExpectationFailed) { }
		public HttpExpectationFailedException(string message) : base(message, StatusCodes.Status417ExpectationFailed) { }
	}

	public sealed class HttpMisdirectedException : CoalescenceHttpException
	{
		public HttpMisdirectedException() : base(StatusCodes.Status421MisdirectedRequest) { }
		public HttpMisdirectedException(string message) : base(message, StatusCodes.Status421MisdirectedRequest) { }
	}

	public sealed class HttpUnprocessableException : CoalescenceHttpException
	{
		public HttpUnprocessableException() : base(StatusCodes.Status422UnprocessableEntity) { }
		public HttpUnprocessableException(string message) : base(message, StatusCodes.Status422UnprocessableEntity) { }
	}

	public sealed class HttpPreconditionRequiredException : CoalescenceHttpException
	{
		public HttpPreconditionRequiredException() : base(StatusCodes.Status428PreconditionRequired) { }
		public HttpPreconditionRequiredException(string message) : base(message, StatusCodes.Status428PreconditionRequired) { }
		public HttpPreconditionRequiredException(object data) : base(data, StatusCodes.Status428PreconditionRequired) { }
	}

	public sealed class HttpNotImplementedException : CoalescenceHttpException
	{
		public HttpNotImplementedException() : base(StatusCodes.Status501NotImplemented) { }
		public HttpNotImplementedException(string message) : base(message, StatusCodes.Status501NotImplemented) { }
	}

}

namespace Microsoft.AspNetCore.Mvc
{
	public static class CoalescenceExceptionExtensions
	{
		public static void BadRequest(this CoalescenceControllerBase controller) => throw new HttpBadRequestException();
		public static void BadRequest(this CoalescenceControllerBase controller, string message) => throw new HttpBadRequestException(message);
		public static void BadRequest(this CoalescenceControllerBase controller, object data) => throw new HttpBadRequestException(data);
		public static void Unauthorized(this CoalescenceControllerBase controller) => throw new HttpUnauthorizedException();
		public static void Unauthorized(this CoalescenceControllerBase controller, string message) => throw new HttpUnauthorizedException(message);
		public static void Forbidden(this CoalescenceControllerBase controller) => throw new HttpForbiddenException();
		public static void Forbidden(this CoalescenceControllerBase controller, string message) => throw new HttpForbiddenException(message);
		public static void NotFound(this CoalescenceControllerBase controller) => throw new HttpNotFoundException();
		public static void NotFound(this CoalescenceControllerBase controller, string message) => throw new HttpNotFoundException(message);
		public static void NotFound(this CoalescenceControllerBase controller, object data) => throw new HttpNotFoundException(data);
		public static void Conflict(this CoalescenceControllerBase controller) => throw new HttpConflictException();
		public static void Conflict(this CoalescenceControllerBase controller, string message) => throw new HttpConflictException(message);
		public static void Conflict(this CoalescenceControllerBase controller, object data) => throw new HttpConflictException(data);
		public static void Gone(this CoalescenceControllerBase controller) => throw new HttpGoneException();
		public static void Gone(this CoalescenceControllerBase controller, string message) => throw new HttpGoneException(message);
		public static void Length(this CoalescenceControllerBase controller) => throw new HttpLengthException();
		public static void Length(this CoalescenceControllerBase controller, string message) => throw new HttpLengthException(message);
		public static void PreconditionFailed(this CoalescenceControllerBase controller) => throw new HttpPreconditionFailedException();
		public static void PreconditionFailed(this CoalescenceControllerBase controller, string message) => throw new HttpPreconditionFailedException(message);
		public static void PreconditionFailed(this CoalescenceControllerBase controller, object data) => throw new HttpPreconditionFailedException(data);
		public static void PayloadSize(this CoalescenceControllerBase controller) => throw new HttpPayloadSizeException();
		public static void PayloadSize(this CoalescenceControllerBase controller, string message) => throw new HttpPayloadSizeException(message);
		public static void Unsupported(this CoalescenceControllerBase controller) => throw new HttpUnsupportedException();
		public static void Unsupported(this CoalescenceControllerBase controller, string message) => throw new HttpUnsupportedException(message);
		public static void ExpectationFailed(this CoalescenceControllerBase controller) => throw new HttpExpectationFailedException();
		public static void ExpectationFailed(this CoalescenceControllerBase controller, string message) => throw new HttpExpectationFailedException(message);
		public static void Misdirected(this CoalescenceControllerBase controller) => throw new HttpMisdirectedException();
		public static void Misdirected(this CoalescenceControllerBase controller, string message) => throw new HttpMisdirectedException(message);
		public static void Unprocessable(this CoalescenceControllerBase controller) => throw new HttpUnprocessableException();
		public static void Unprocessable(this CoalescenceControllerBase controller, string message) => throw new HttpUnprocessableException(message);
		public static void PreconditionRequired(this CoalescenceControllerBase controller) => throw new HttpPreconditionRequiredException();
		public static void PreconditionRequired(this CoalescenceControllerBase controller, string message) => throw new HttpPreconditionRequiredException(message);
		public static void PreconditionRequired(this CoalescenceControllerBase controller, object data) => throw new HttpPreconditionRequiredException(data);
		public static void NotImplemented(this CoalescenceControllerBase controller) => throw new HttpNotImplementedException();
		public static void NotImplemented(this CoalescenceControllerBase controller, string message) => throw new HttpNotImplementedException(message);
	}
}
