namespace EllipticBit.Hotwire.Shared
{
	public readonly struct MultipartContentItem<T>
	{
		public readonly T Content;
		public readonly string Name;
		public readonly string FileName;
		public readonly string ContentType;

		public MultipartContentItem(T content, string contentType, string name, string fileName = null)
		{
			this.Content = content;
			this.Name = name;
			this.FileName = fileName;
			this.ContentType = contentType;
		}
	};
}
