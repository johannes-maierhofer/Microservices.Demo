using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Argo.MD.BuildingBlocks.EfCore.Conventions
{
    // see https://www.codemag.com/Article/2211072/EF-Core-7-It-Just-Keeps-Getting-Better
    public class MaxStringLength200Convention : IModelFinalizingConvention
    {
        public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder,
            IConventionContext<IConventionModelBuilder> context)
        {
            foreach (var property in modelBuilder.Metadata.GetEntityTypes()
                         .SelectMany(entityType =>
                             entityType.GetDeclaredProperties().Where(
                                 property => property.ClrType == typeof(string))))
            {
                property.Builder.HasMaxLength(200);
            }
        }
    }
}
