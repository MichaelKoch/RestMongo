using MongoBase.Controllers;
using Sample.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.Controllers
{

    public class ArticleVariantController : FeedController<ArticleVariant>
    {
        private ProductContext _context;
        public ArticleVariantController(ProductContext context) : base(context.ArticleVariants)
        {
            this._context = context;
        }

        protected override async Task<bool> LoadRelations(IList<ArticleVariant> values, IList<string> relations)
        {
            var waitFor = new List<Task>();
            if (relations.Contains("SalesText"))
            {
                relations.Remove("SalesText");
                waitFor.Add(loadSalesText(values));
            }
            if (relations.Contains("Compositions"))
            {
                relations.Remove("Compositions");
                waitFor.Add(loadCompositions(values));
            }
            if (relations.Contains("Attributes"))
            {
                relations.Remove("Attributes");
                waitFor.Add(loadAttributes(values));
            }



            Task.WaitAll(waitFor.ToArray());
            if (relations.Count > 0 && relations[0] != "")
            {
                throw new NotSupportedException("INVALID EXPAND:" + string.Join(" ", relations));
            }
            return true;

        }
        private async Task<bool> loadSalesText(IList<ArticleVariant> values)
        {
            var queryable = _context.MaterialTexts.AsQueryable();
            var materialNumbers = values.Select(c => c.MaterialNumber).ToList();
            var relValues = queryable.Where(mt =>
                mt.Locale == "de-DE" &&
                materialNumbers.Contains(mt.MaterialNumber)
            ).ToList();
            foreach (var av in values)
            {
                av.SalesText = relValues.Where(mt => mt.MaterialNumber == av.MaterialNumber).FirstOrDefault();
            }
            return true;
        }

        private async Task<bool> loadAttributes(IList<ArticleVariant> values)
        {
            var queryable = _context.MaterialClassifications.AsQueryable();
            var materialNumbers = values.Select(c => c.MaterialNumber).ToList();
            var relValues = queryable.Where(mt =>
                  mt.Locale == "de-DE" &&
                  materialNumbers.Contains(mt.MaterialNumber)
                ).ToList();
            foreach (var av in values)
            {
                av.Attributes = relValues.Where(mt => mt.MaterialNumber == av.MaterialNumber).ToList();
            }
            return true;
        }

        private async Task<bool> loadCompositions(IList<ArticleVariant> values)
        {
            var queryable = _context.MaterialCompositions.AsQueryable();
            var materialNumbers = values.Select(c => c.MaterialNumber).ToList();
            var relValues = queryable.Where(
                mt => mt.Locale == "de-DE" &&
                materialNumbers.Contains(mt.MaterialNumber)
                ).ToList();
            foreach (var av in values)
            {
                av.Compositions = relValues.Where(mt => mt.MaterialNumber == av.MaterialNumber).ToList();
            }
            return true;
        }
    }
}
