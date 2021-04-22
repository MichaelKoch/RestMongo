using MongoBase.Controllers;
using Sample.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.Controllers
{

    public class ArticleVariantController : LocalizedFeedController<ArticleVariant>
    {
        private ProductContext _context;
        public ArticleVariantController(ProductContext context) : base(context.ArticleVariants)
        {
            this._context = context;
        }

        protected override async Task<bool> LoadRelations(IList<ArticleVariant> values, IList<string> relations,string locale)
        {
            var waitFor = new List<Task>();
            foreach (var av in values)
            {
                av.Locale = locale;
            }
            if (relations.Contains("SalesText"))
            {
                relations.Remove("SalesText");
                waitFor.Add(loadSalesText(values,locale));
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
            return true;

        }
        private async Task<bool> loadSalesText(IList<ArticleVariant> values,string locale)
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
                av.SalesText = relValues.Where(mt => mt.MaterialNumber == av.MaterialNumber).FirstOrDefault();
            }
            return true;
        }

        private async Task<bool> loadAttributes(IList<ArticleVariant> values, string locale)
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
                av.Attributes = relValues.Where(mt => mt.MaterialNumber == av.MaterialNumber).ToList();
            }
            return true;
        }

        private async Task<bool> loadCompositions(IList<ArticleVariant> values, string locale)
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
                av.Compositions = relValues.Where(mt => mt.MaterialNumber == av.MaterialNumber).ToList();
            }
            return true;
        }
    }
}
