using System.Collections.Generic;
using System.Linq;

namespace CompetingConsumers.Common
{
    public abstract class MapperBase<TFrom, TTo> : IMapper<TFrom, TTo>
    {
        public abstract TTo Map(TFrom source);

        public IEnumerable<TTo> Map(IEnumerable<TFrom> source)
        {
            return source.Select(Map);
        }
    }
}