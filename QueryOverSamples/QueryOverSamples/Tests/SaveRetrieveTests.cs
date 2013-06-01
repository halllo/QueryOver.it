using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Criterion;
using QueryOverSamples.Domain;
using System.Linq;

namespace QueryOverSamples.Tests
{
	public abstract class SaveRetrieveTests
	{
		protected abstract ISession Session { get; }

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

		[TestMethod]
		public void WhereCondition_Conjunction()
		{
			Session.Save(new Person { Name = "Ascott", FirstName = "Abigail" });
			Session.Save(new Person { Name = "Ascott", FirstName = "Arthur" });

			var query = Session.QueryOver<Person>();
			var persons = query.Where(p => p.Name == "Ascott")
							   .List();

			Assert.AreEqual(2, persons.Count);
			query = null;
			persons = null;

			//conjunction of WHERE conditions: .And(), .Where() and && are equivalent
			query = Session.QueryOver<Person>();
			persons = query.Where(p => p.Name == "Ascott")
						   .And(p => p.FirstName == "Abigail")
						   .List();

			Assert.AreEqual(1, persons.Count);
			query = null;
			persons = null;

			query = Session.QueryOver<Person>();
			persons = query.Where(p => p.Name == "Ascott")
						   .Where(p => p.FirstName == "Abigail")
						   .List();

			Assert.AreEqual(1, persons.Count);
			query = null;
			persons = null;

			query = Session.QueryOver<Person>();
			persons = query.Where(p => p.Name == "Ascott" && p.FirstName == "Abigail")
						   .List();

			Assert.AreEqual(1, persons.Count);
			query = null;
			persons = null;
		}

		[TestMethod]
		public void WhereCondition_Disjunction()
		{
			Session.Save(new Person { Name = "Ascott", FirstName = "Abigail" });
			Session.Save(new Person { Name = "Birchwood", FirstName = "Brandon" });
			Session.Save(new Person { Name = "Cinnamon", FirstName = "Cynthia" });

			var query = Session.QueryOver<Person>();
			var persons = query.Where(p => p.Name == "Ascott" || p.Name == "Birchwood")
							   .List();

			Assert.AreEqual(2, persons.Count);
			query = null;
			persons = null;

			query = Session.QueryOver<Person>();
			persons = query.Where(Restrictions.Disjunction()
											  .Add<Person>(p => p.Name == "Ascott")
											  .Add<Person>(p => p.Name == "Birchwood"))
						   .List();

			Assert.AreEqual(2, persons.Count);
			query = null;
			persons = null;

			var disjunction = new Disjunction();
			disjunction.Add<Person>(p => p.Name == "Ascott");
			disjunction.Add<Person>(p => p.Name == "Birchwood");

			query = Session.QueryOver<Person>();
			persons = query.Where(disjunction)
						   .List();

			Assert.AreEqual(2, persons.Count);
			query = null;
			persons = null;
		}
	}
}
