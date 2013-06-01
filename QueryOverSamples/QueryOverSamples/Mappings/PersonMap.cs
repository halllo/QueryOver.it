using FluentNHibernate.Mapping;
using QueryOverSamples.Domain;

namespace QueryOverSamples.Mappings
{
	public class PersonMap : ClassMap<Person>
	{
		public PersonMap()
		{
			Table("Person");

			Id(p => p.Id).GeneratedBy.Identity();
			Map(p => p.Name);
			Map(p => p.FirstName);
			Map(p => p.DateOfBirth);
		}
	}
}
