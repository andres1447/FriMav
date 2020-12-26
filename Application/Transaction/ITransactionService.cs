﻿using FriMav.Domain;
using FriMav.Domain.Entities;
using FriMav.Domain.Proyections;
using System.Collections.Generic;

namespace FriMav.Application
{
    public interface ITransactionService
    {
        IEnumerable<TransactionDocument> GetAll();

        TransactionDocument Get(int transactionId);

        IEnumerable<TransactionEntry> GetAllByPersonId(int personId, int offset = 0, int count = 20);

        [Transactional]
        void Create(CreatePayment payment);
    }
}
