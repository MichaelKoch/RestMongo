using MongoBase.Interfaces;
using MongoBase.Utils;
using Sample.Domain.Models.Enities;
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

        private readonly IRepository<ReadWriteEntity> _readWriteEntities;

        public ProductContext(IConnectionSettings settings)
        {
           
            _articleVariants = new ArticleVariantRepository(settings,this);
            _materialClassifications = new MaterialClassificationRepository(settings, this);
            _materialCompositions = new MaterialCompositionRepository(settings, this);
            _materialTexts = new MaterialTextRepository(settings, this);
            _collectionMaterials = new CollectionMaterialRepository(settings, this);
            _readWriteEntities = new MongoBase.Repositories.MongoRepository<ReadWriteEntity>(settings);
        }

        public ArticleVariantRepository ArticleVariants => _articleVariants;

        public MaterialClassificationRepository MaterialClassifications => _materialClassifications;

        public MaterialCompositionRepository MaterialCompositions => _materialCompositions;

        public MaterialTextRepository MaterialTexts => _materialTexts;

        public CollectionMaterialRepository CollectionMaterials => _collectionMaterials;

        public IRepository<ReadWriteEntity> ReadWriteEntities => _readWriteEntities;
    }
}
