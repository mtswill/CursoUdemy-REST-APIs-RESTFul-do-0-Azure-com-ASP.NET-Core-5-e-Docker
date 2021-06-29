using CursoUdemy.Hypermedia.Abstract;
using System.Collections.Generic;

namespace CursoUdemy.Hypermedia.Filters
{
    public class HyperMediaFilterOptions
    {
        public List<IResponseEnricher> ContentResponseEnricherList { get; set; } = new List<IResponseEnricher>();
    }
}
