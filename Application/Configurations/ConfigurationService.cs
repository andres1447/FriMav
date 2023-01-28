using FriMav.Domain;
using FriMav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FriMav.Application.Configurations
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IRepository<ConfigValue> _repository;

        public ConfigurationService(IRepository<ConfigValue> repository)
        {
            _repository = repository;
        }

        public decimal GetDecimal(string code)
        {
            return _repository.Query().Where(x => x.Code == code).Select(x => x.DecimalValue).DefaultIfEmpty().First();
        }

        public string GetString(string code)
        {
            return _repository.Query().Where(x => x.Code == code).Select(x => x.StringValue).DefaultIfEmpty().First();
        }

        public int GetInt(string code)
        {
            return _repository.Query().Where(x => x.Code == code).Select(x => x.IntValue).DefaultIfEmpty().First();
        }

        public void Update(ConfigValue updated)
        {
            var current = _repository.Query().First(x => x.Code == updated.Code);
            switch (current.Type)
            {
                case ConfigurationType.Int: current.IntValue = updated.IntValue; break;
                case ConfigurationType.String: current.StringValue = updated.StringValue; break;
                case ConfigurationType.Decimal: current.DecimalValue = updated.DecimalValue; break;
                default: throw new Exception();
            }
        }

        public List<ConfigValue> GetAll() => _repository.GetAll();
    }
}
