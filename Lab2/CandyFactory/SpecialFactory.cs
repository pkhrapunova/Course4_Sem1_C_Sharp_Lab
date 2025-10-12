namespace CandyFactory
{
	public class SpecialFactory<T> : Factory<T> where T : CandyBase
	{
		public string Specialization { get; }

		public SpecialFactory(string specialization) : base(specialization)
		{
			Specialization = specialization;
		}
	}
}