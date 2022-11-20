using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;

namespace UltimatR
{
    public class IgnoreApiDocument : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach(var apiDescription in context.ApiDescriptions)
            {
                apiDescription.TryGetMethodInfo(out MethodInfo info);
                if(info.GetCustomAttributes<IgnoreApiAttribute>().Distinct().Any())
                {
                    string kepath = apiDescription.RelativePath;
                    var remRoutes = swaggerDoc.Paths
                                        .Where(x => x.Key.ToLower()
                                        .Contains(kepath.ToString()
                                        .ToLower())).ToArray();

                  var a = remRoutes.ForEach(x => swaggerDoc.Paths.Remove(x.Key)).ToList();
                }
            }
        }
    }
}

