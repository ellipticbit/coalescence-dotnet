namespace EllipticBit.Lexicon.Client
{
	public  interface ILexiconRequestFactory
	{
		ILexiconRequest CreateRequest();
		ILexiconRequest CreateRequest(string name);
	}
}
