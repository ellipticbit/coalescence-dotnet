namespace EllipticBit.Lexicon.Client
{
	public interface ILexiconRequestFactoryBuilder
	{
		ILexiconRequestFactoryBuilder AddLexiconRequestFactory(string name, LexiconRequestOptions options);
	}
}
