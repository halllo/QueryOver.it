﻿using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using QueryOverSamples.Domain;
using System.Linq;

namespace QueryOverSamples
{
	[TestClass]
	public class Tests
	{
		private ISession Session;

		[TestInitialize]
		public void Initialize()
		{
			var connectionString = new ConnectionString("..\\..\\Database.sdf");
			var config = Fluently.Configure()
				.Database(MsSqlCeConfiguration.Standard.ConnectionString(connectionString.ToString()))
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<Person>())
				.ExposeConfiguration(x => x.SetProperty("connection.release_mode", "on_close"))
				.BuildConfiguration();

			connectionString.CreateDatabase(config);

			var sessions = config.BuildSessionFactory();
			Session = sessions.OpenSession();
		}

		[TestCleanup]
		public void Cleanup()
		{
			Session.Dispose();
		}

		[TestMethod]
		public void SaveAndRetrieve()
		{
			Session.Save(new Person { Name = "Max" });

			var query = Session.QueryOver<Person>();
			var persons = query.List();

			Assert.IsTrue(persons.Any(p => p.Name == "Max"));
		}
	}
}
