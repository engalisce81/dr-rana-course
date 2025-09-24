﻿using Dev.Acadmy.Universites;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Dev.Acadmy.Data.Seeds
{
    public class TermDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Term, Guid> _termRepository;

        public TermDataSeedContributor(IRepository<Term, Guid> termRepository)
        {
            _termRepository = termRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            var terms = new List<Term>
        {
            new Term { Name = "First Term", IsActive = true },
            new Term { Name = "Seconed Term", IsActive = false },
        };

            foreach (var term in terms)
            {
                var existing = await _termRepository.FirstOrDefaultAsync(x => x.Name == term.Name);
                if (existing == null)
                {
                    await _termRepository.InsertAsync(term);
                }
                else
                {
                    // اعمل update على الكائن الموجود
                    existing.IsActive = term.IsActive;
                    await _termRepository.UpdateAsync(existing);
                }
            }
        }
    }
}
