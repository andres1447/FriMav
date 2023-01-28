using FriMav.Domain;
using FriMav.Domain.Entities;
using System.Collections.Generic;

namespace FriMav.Application.Configurations
{
    public interface IConfigurationService
    {
        decimal GetDecimal(string code);
        int GetInt(string code);
        string GetString(string code);
        [Transactional]
        void Update(ConfigValue updated);
        List<ConfigValue> GetAll();
    }
}