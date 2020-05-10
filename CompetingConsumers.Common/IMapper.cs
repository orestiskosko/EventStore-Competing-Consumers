using System.Collections.Generic;

namespace CompetingConsumers.Common
{
    public interface IMapper<TFrom, TTo>
    {
        TTo Map(TFrom source);
        IEnumerable<TTo> Map(IEnumerable<TFrom> source);
    }
}