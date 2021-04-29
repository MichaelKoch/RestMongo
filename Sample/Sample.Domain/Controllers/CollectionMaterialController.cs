using RestMongo.Controllers;
using RestMongo.Utils;
using Sample.Domain.Models.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.Controllers
{
    public class CollectionMaterialController : LocalizedFeedController<CollectionMaterial, CollectionMaterialDTO>
    {
        private ProductContext _context;
        public CollectionMaterialController(ProductContext context) : base(context.CollectionMaterials, 1000)
        {
            this._context = context;

        }

        protected override CollectionMaterialDTO ConvertToDTO(CollectionMaterial value)
        {
            return CopyUtils<CollectionMaterialDTO>.Convert(value);
        }


        protected async override Task<IList<CollectionMaterialDTO>> LoadRelations(IList<CollectionMaterialDTO> values, IList<string> relations, string locale)
        {
            if (relations == null || relations.Count == 0)
            {
                relations = new List<string>() { "Compositions", "Attributes", "SalesText" };
            }
            var waitFor = new List<Task>();

            foreach (var av in values)
            {
                av.Locale = locale;
            }
            if (relations.Contains("SalesText"))
            {
                relations.Remove("SalesText");
                waitFor.Add(loadSalesText(values, locale));
            }
            if (relations.Contains("Compositions"))
            {
                relations.Remove("Compositions");
                waitFor.Add(loadCompositions(values, locale));
            }
            if (relations.Contains("Attributes"))
            {
                relations.Remove("Attributes");
                waitFor.Add(loadAttributes(values, locale));
            }
            if (relations.Contains("Variants"))
            {
                relations.Remove("Variants");
                waitFor.Add(loadVariants(values, locale));
            }
            Task.WaitAll(waitFor.ToArray());
            if (relations.Count > 0 && relations[0] != "")
            {
                throw new NotSupportedException("INVALID EXPAND:" + string.Join(" ", relations));
            }
            return values;

        }
        private async Task<bool> loadSalesText(IList<CollectionMaterialDTO> values, string locale)
        {
            var queryable = _context.MaterialTexts.AsQueryable();
            var materialNumbers = values.Select(c => c.MaterialNumber).ToList();
            var relValues = queryable.Where(mt =>
                mt.Locale == locale &&
                materialNumbers.Contains(mt.MaterialNumber)
            ).ToList();
            foreach (var av in values)
            {
                av.Locale = locale;
                av.SalesText = CopyUtils<MaterialTextDTO>.Convert(relValues.Where(mt => mt.MaterialNumber == av.MaterialNumber).FirstOrDefault());
            }
            return true;
        }

        private async Task<bool> loadVariants(IList<CollectionMaterialDTO> values, string locale)
        {
            var queryable = _context.ArticleVariants.AsQueryable();
            var materialNumbers = values.Select(c => c.MaterialNumber).ToList();
            var relValues = queryable.Where(mt =>
                  materialNumbers.Contains(mt.MaterialNumber)
                ).ToList();

            foreach (var variant in relValues)
            {
                variant.Locale = locale;
            }

            foreach (var av in values)
            {
                av.Locale = locale;
                av.Variants = CopyUtils<List<ArticleVariantDTO>>.Convert(relValues.Where(mt => mt.MaterialNumber == av.MaterialNumber).ToList());
            }
            return true;
        }

        private async Task<bool> loadAttributes(IList<CollectionMaterialDTO> values, string locale)
        {
            var queryable = _context.MaterialClassifications.AsQueryable();
            var materialNumbers = values.Select(c => c.MaterialNumber).ToList();
            var relValues = queryable.Where(mt =>
                  mt.Locale == locale &&
                  materialNumbers.Contains(mt.MaterialNumber)
                ).ToList();
            foreach (var av in values)
            {
                av.Locale = locale;
                av.Attributes = CopyUtils<List<MaterialClassificationDTO>>.Convert(relValues.Where(mt => mt.MaterialNumber == av.MaterialNumber).ToList());
            }
            return true;
        }

        private async Task<bool> loadCompositions(IList<CollectionMaterialDTO> values, string locale)
        {
            var queryable = _context.MaterialCompositions.AsQueryable();
            var materialNumbers = values.Select(c => c.MaterialNumber).ToList();
            var relValues = queryable.Where(
                mt => mt.Locale == locale &&
                materialNumbers.Contains(mt.MaterialNumber)
                ).ToList();
            foreach (var av in values)
            {
                av.Locale = locale;
                av.Compositions = CopyUtils<List<MaterialCompositionDTO>>.Convert(relValues.Where(mt => mt.MaterialNumber == av.MaterialNumber).ToList());
            }
            return true;
        }
    }
}
