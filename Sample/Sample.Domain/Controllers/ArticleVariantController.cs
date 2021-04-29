using Microsoft.AspNetCore.Mvc;
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

    public class ArticleVariantController : LocalizedFeedController<ArticleVariant, ArticleVariantDTO>
    {
        private ProductContext _context;
        public ArticleVariantController(ProductContext context) : base(context.ArticleVariants, 10000)
        {
            this._context = context;
        }

        protected override async Task<IList<ArticleVariantDTO>> LoadRelations(IList<ArticleVariantDTO> values, IList<string> relations, string locale)
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



            Task.WaitAll(waitFor.ToArray());
            if (relations.Count > 0 && relations[0] != "")
            {
                throw new NotSupportedException("INVALID EXPAND:" + string.Join(" ", relations));
            }
            return values;

        }
        private async Task<bool> loadSalesText(IList<ArticleVariantDTO> values, string locale)
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

        private async Task<bool> loadAttributes(IList<ArticleVariantDTO> values, string locale)
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

        private async Task<bool> loadCompositions(IList<ArticleVariantDTO> values, string locale)
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


        protected override ArticleVariantDTO ConvertToDTO(ArticleVariant value)
        {
            return CopyUtils<ArticleVariantDTO>.Convert(value);
        }
    }
}
