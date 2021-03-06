﻿using FriMav.Domain.Entities.Payrolls;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings.Payrolls
{
    public class LoanFeeMapping : EntityTypeConfiguration<LoanFee>
    {
        public LoanFeeMapping()
        {
            HasKey(x => x.Id);
        }
    }
}
