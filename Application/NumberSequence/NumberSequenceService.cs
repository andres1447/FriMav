using FriMav.Domain;
using FriMav.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public class NumberSequenceService : INumberSequenceService
    {
        private INumberSequenceRepository _numberSequenceRepository;

        public NumberSequenceService(INumberSequenceRepository numberSequenceRepository)
        {
            _numberSequenceRepository = numberSequenceRepository;
        }
        public int NextOrInit(string key)
        {
            var sequence = _numberSequenceRepository.FindBy(x => x.Key == key);
            if (sequence == null)
            {
                sequence = new NumberSequence { Key = key, CurrentId = 1 };
                _numberSequenceRepository.Create(sequence);
            }
            else
            {
                sequence.CurrentId++;
                _numberSequenceRepository.Update(sequence);
            }
            _numberSequenceRepository.Save();
            return sequence.CurrentId;
        }
    }
}
