namespace EllipticBit.Lexicon.Client
{
	public interface ILexiconRequest
	{
		ILexiconRequestBuilder Get();
		ILexiconRequestBuilder Put();
		ILexiconRequestBuilder Post();
		ILexiconRequestBuilder Patch();
		ILexiconRequestBuilder Delete();
		ILexiconRequestBuilder Head();
	}
}
