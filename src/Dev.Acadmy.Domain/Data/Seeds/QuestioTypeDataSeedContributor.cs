using Dev.Acadmy;
using Dev.Acadmy.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
namespace Course.Data.Seeds
{
    public class QuestioTypeDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<QuestionType, Guid> _questionTypeRepository;
        public QuestioTypeDataSeedContributor(IRepository<QuestionType, Guid> questionTypeRepository)
        {
            _questionTypeRepository = questionTypeRepository;
        }
        public async Task SeedAsync(DataSeedContext context)
        {
            List<QuestionType> questiotypes = new List<QuestionType>
            {
                new QuestionType(){Name = QuestionTypeConsts.MCQ,Key=(int)QuestionTypeEnum.MCQ},
                new QuestionType(){Name = QuestionTypeConsts.TrueOrFalse,Key=(int)QuestionTypeEnum.TrueOrFalse},
                new QuestionType(){Name = QuestionTypeConsts.CompleteAnswer,Key=(int)QuestionTypeEnum.CompleteAnswer},
                new QuestionType(){Name = QuestionTypeConsts.ShortAnswer,Key=(int)QuestionTypeEnum.ShortAnswer},

            };
            foreach (var questiotype in questiotypes)
            {
                var questiotypesDb = await _questionTypeRepository.GetListAsync();
                if (!questiotypesDb.Any(r => r.Key == questiotype.Key)) await _questionTypeRepository.InsertAsync(questiotype);
            }
        }
    }
}
