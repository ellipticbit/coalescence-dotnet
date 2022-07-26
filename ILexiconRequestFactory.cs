namespace EllipticBit.Lexicon.Client
{
	public  interface ILexiconRequestFactory
	{
		ILexiconRequest CreateLexiconRequest();
		ILexiconRequest CreateLexiconRequest(string name);
	}
}
