using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EllipticBit.Hotwire.AspNetCore
{
	public abstract class HotwireControllerBase : ControllerBase
	{
		private readonly HotwireControllerOptions _options;

		protected HotwireControllerBase(HotwireControllerOptions options)
		{
			_options = options;
		}

		protected async Task<T> MultipartAsSerialized<T>(string name)
		{
			var file = this.Request.Form.Files.GetFile(name);
			if (file == null) throw new ArgumentOutOfRangeException(nameof(name), $"Unable to locate multipart content item with name: {name}");

			var serializer = this._options.GetSerializer(file.ContentType);

			await using var stream = file.OpenReadStream();
			using var reader = new StreamReader(stream, Encoding.UTF8, true);
			var serialized = await reader.ReadToEndAsync();

			return await serializer.Deserialize<T>(serialized);
		}

		protected Task<string> MultipartAsText(string name)
		{
			var file = this.Request.Form.Files.GetFile(name);
			if (file == null) throw new ArgumentOutOfRangeException(nameof(name), $"Unable to locate multipart content item with name: {name}");

			using var stream = file.OpenReadStream();
			using var reader = new StreamReader(stream, Encoding.UTF8, true);
			return reader.ReadToEndAsync();
		}

		protected MultipartContentItem<Stream> MultipartAsStream(string name)
		{
			var file = this.Request.Form.Files.GetFile(name);
			if (file == null) throw new ArgumentOutOfRangeException(nameof(name), $"Unable to locate multipart content item with name: {name}");

			return new MultipartContentItem<Stream>(file.OpenReadStream(), file.ContentType, file.Name, file.FileName);
		}

		protected async Task<MultipartContentItem<byte[]>> MultipartAsByteArray(string name, int maxSize = Int32.MaxValue)
		{
			var file = this.Request.Form.Files.GetFile(name);
			if (file == null) throw new ArgumentOutOfRangeException(nameof(name), $"Unable to locate multipart content item with name: {name}");

			if (file.Length >= Int32.MaxValue) throw new OverflowException($"Unable to load stream '{name}' into memory. File size must be less than {Int32.MaxValue} bytes.");
			if (file.Length >= maxSize) throw new OverflowException($"Unable to load stream '{name}' into memory. File size must be less than {maxSize} bytes.");

			using var stream = file.OpenReadStream();
			using var reader = new MemoryStream(Convert.ToInt32(file.Length));
			await stream.CopyToAsync(reader);

			return new MultipartContentItem<byte[]>(reader.ToArray(), file.ContentType, file.Name, file.FileName);
		}
	}
}
