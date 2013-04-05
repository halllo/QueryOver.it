using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryOverSamples.Domain;
using System.Linq;

namespace QueryOverSamples
{
	[TestClass]
	public class SimpleTests : SessionTests
	{
		[TestMethod]
		public void SaveAndRetrieve()
		{
			Session.Save(new Person { Name = "Max" });

			var query = Session.QueryOver<Person>();
			var persons = query.List();

			Assert.IsTrue(persons.Any(p => p.Name == "Max"));
		}

		[TestMethod]
		public void RowCount()
		{
			Session.Save(new Person { Name = "Max1" });
			Session.Save(new Person { Name = "Max2" });

			var query = Session.QueryOver<Person>();
			var personsCount = query.RowCount();

			Assert.AreEqual(2, personsCount);
		}
	}
}
