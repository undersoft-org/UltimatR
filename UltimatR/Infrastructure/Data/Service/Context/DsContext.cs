using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Client;
using Microsoft.OData.Edm;
using System;

namespace UltimatR
{
    public partial class DsContext<TStore> : DsContext where TStore : IDataStore
    {
        public DsContext(Uri serviceUri) : base(serviceUri)
        {
        }       
    }

    public partial class DsContext : DataServiceContext, IDataClient 
    {
        public DsContext(Uri serviceUri) : base(serviceUri)
        {
            MergeOption = MergeOption.AppendOnly;
            HttpRequestTransportMode = HttpRequestTransportMode.HttpClient;
            IgnoreResourceNotFoundException = true;
            KeyComparisonGeneratesFilterQuery = false;
            DisableInstanceAnnotationMaterialization = true;
            EnableWritingODataAnnotationWithoutPrefix = false;
            AddAndUpdateResponsePreference = DataServiceResponsePreference.NoContent;
            EntityParameterSendOption = EntityParameterSendOption.SendOnlySetProperties;
            SaveChangesDefaultOptions = SaveChangesOptions.BatchWithSingleChangeset;
            ResolveName = (t) => this.GetMappedName(t);
            ResolveType = (n) => this.GetMappedType(n);
        }

        public void CreateServiceModel()
        {          
            Format.LoadServiceModel = () => GetServiceModel();
            Format.UseJson();
        }

        public IEdmModel GetServiceModel()
        {
            Type t = GetType();
            if (!DsCatalog.EdmModels.TryGet(t, out IEdmModel edmModel))
                 DsCatalog.EdmModels.Add(t, edmModel = OnModelCreating(this.GetEdmModel()));
            return edmModel;
        }

        protected virtual IEdmModel OnModelCreating(IEdmModel builder)
        {
            return builder;
        }
    }
}