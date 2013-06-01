using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using QueryOverSamples.Connectivity;
using QueryOverSamples.Domain;
using QueryOverSamples.Tests;

namespace QueryOverSamples
{
	[TestClass]
	public class TestsForSqlCe : SaveRetrieveTests
	{
		ISession _Session;
		protected override ISession Session
		{
			get { return _Session; }
		}

		[TestInitialize]
		public void Initialize()
		{
			var connectionString = new ConnectionString("..\\..\\Database.sdf");
			var config = Fluently.Configure()
								 .Database(MsSqlCeConfiguration.Standard.ConnectionString(connectionString.ToString()))
								 .Mappings(m => m
													.FluentMappings.AddFromAssemblyOf<Person>()
													.Conventions.Add(DefaultLazy.Never()))
								 .ExposeConfiguration(x => x.SetProperty("connection.release_mode", "on_close"))
								 .BuildConfiguration();

			connectionString.CreateDatabase(config);

			var sessions = config.BuildSessionFactory();
			_Session = sessions.OpenSession();
		}

		[TestCleanup]
		public void Cleanup()
		{
			Session.Dispose();
		}
	}
}