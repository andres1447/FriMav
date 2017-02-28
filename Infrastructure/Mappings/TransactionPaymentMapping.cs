using FriMav.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Mappings
{
    public class TransactionPaymentMapping : BaseMapping<TransactionPayment>
    {
        public override void Mapping()
        {
            ToTable("TransactionPayment");
            HasKey(t => new { t.SourceTransactionId, t.TargetTransactionId });
            HasRequired(t => t.SourceTransaction)
                .WithMany()
                .HasForeignKey(t => t.SourceTransactionId)
                .WillCascadeOnDelete(true);
            HasRequired(t => t.TargetTransaction)
                .WithMany(t => t.Payments)
                .HasForeignKey(t => t.TargetTransactionId)
                .WillCascadeOnDelete(false);
        }
    }
}
