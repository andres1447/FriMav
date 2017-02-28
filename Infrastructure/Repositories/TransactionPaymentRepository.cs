using FriMav.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using FriMav.Domain.Repositories;
using System.Data.SqlClient;
using Dapper;
using FriMav.Domain.Proyections;
using System;

namespace FriMav.Infrastructure.Repositories
{
    public class TransactionPaymentRepository : BaseRepository<TransactionPayment>, ITransactionPaymentRepository
    {
        public TransactionPaymentRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
