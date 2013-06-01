using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using QueryOverSamples.Tests;

namespace QueryOverSamples
{
	[TestClass]
	public class TestsForMsSql : SaveRetrieveTests
	{
		protected override ISession Session
		{
			get { return null; }
		}
	}
}
