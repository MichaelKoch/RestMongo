using Microsoft.AspNetCore.Mvc;
using RestMongo.Interfaces;
using RestMongo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestMongo.Controllers
{

    [Route("[controller]")]
    public abstract class  SearchController<TEntity, TDataTransfer> : ControllerBase
            where TEntity : BaseDocument
            where TDataTransfer : class
    {
        protected IRepository<TEntity> _repository;
        protected int _maxPageSize = 0; //TODO => get it from configuration

        public SearchController(IRepository<TEntity> repository, int maxPageSize = 1000)
        {
            _maxPageSize = maxPageSize;
            _repository = repository;
        }
    }
}
