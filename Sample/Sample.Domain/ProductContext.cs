using MongoBase.Interfaces;
using MongoBase.Utils;
using Sample.Domain.Repositories;

namespace Sample.Domain
{

    public class ProductContext 
    {
        private readonly ArticleVariantRepository _articleVariants;
        private readonly MaterialClassificationRepository _materialClassifications;
        private readonly MaterialCompositionRepository _materialCompositions;
        private readonly MaterialTextRepository _materialTexts;
        private readonly CollectionMaterialRepository _collectionMaterials;



        public ProductContext(IConnectionSettings settings)
        {
           
            _articleVariants = new ArticleVariantRepository(settings,this);
            _materialClassifications = new MaterialClassificationRepository(settings, this);
            _materialCompositions = new MaterialCompositionRepository(settings, this);
            _materialTexts = new MaterialTextRepository(settings, this);
            _collectionMaterials = new CollectionMaterialRepository(settings, this);
        }

        public ArticleVariantRepository ArticleVariants => _articleVariants;

        public MaterialClassificationRepository MaterialClassifications => _materialClassifications;

        public MaterialCompositionRepository MaterialCompositions => _materialCompositions;

        public MaterialTextRepository MaterialTexts => _materialTexts;

        public CollectionMaterialRepository CollectionMaterials => _collectionMaterials;
    }
}
