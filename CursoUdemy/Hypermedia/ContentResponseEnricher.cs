using CursoUdemy.Hypermedia.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CursoUdemy.Hypermedia
{
    public abstract class ContentResponseEnricher<T> : IResponseEnricher where T : ISupportsHyperMedia
    {
        protected abstract Task EnrichModel(T content, IUrlHelper urlHelper);

        public ContentResponseEnricher() {}

        public bool CanEnrich(Type contentType)
        {
            return contentType.Equals(typeof(T)) || contentType.Equals(typeof(List<T>));
        }

        bool IResponseEnricher.CanEnrich(ResultExecutingContext response)
        {
            if (response.Result is OkObjectResult okObjectResult)
                return CanEnrich(okObjectResult.Value.GetType());

            return false;
        }

        public async Task Enrich(ResultExecutingContext response)
        {
            var urlHelper = new UrlHelperFactory().GetUrlHelper(response);

            if (response.Result is OkObjectResult okObjectResult)
            {
                if (okObjectResult.Value is T model)
                {
                    Task.WaitAll(EnrichModel(model, urlHelper));
                }
                else if (okObjectResult.Value is List<T> collection)
                {
                    var bag = new ConcurrentBag<T>(collection);

                    Parallel.ForEach(bag, (element) =>
                    {
                        EnrichModel(element, urlHelper);
                    });
                }
            }
            
            await Task.FromResult<object>(null);
        }
    }
}
