using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SealabAPI.Base;
using SealabAPI.DataAccess.Entities;
using SealabAPI.DataAccess.Models;

namespace SealabAPI.DataAccess.Services
{
    public interface IPreTestAnswerService : IBaseService<PreTestAnswer>
    {

    }
    public class PreTestAnswerService : BaseService<PreTestAnswer>, IPreTestAnswerService
    {
        public PreTestAnswerService(AppDbContext appDbContext) : base(appDbContext) { }
        public override List<TModel> GetAll<TModel>(Expression<Func<PreTestAnswer, bool>> expression = null)
        {
            List<DetailPreTestAnswerResponse> models = new();
            List<IGrouping<Guid, PreTestAnswer>> entities;
            if (expression == null)
                entities = _appDbContext.Set<PreTestAnswer>()
                                        .Include(x => x.Student).ThenInclude(x => x.User)
                                        .Include(x => x.Option).ThenInclude(x => x.Question).ThenInclude(x => x.Module)
                                        .AsNoTracking().AsEnumerable()
                                        .GroupBy(x => x.IdStudent).ToList();
            else
                entities = _appDbContext.Set<PreTestAnswer>()
                                        .Include(x => x.Student).ThenInclude(x => x.User)
                                        .Include(x => x.Option).ThenInclude(x => x.Question).ThenInclude(x => x.Module)
                                        .Where(expression).AsNoTracking().AsEnumerable()
                                        .GroupBy(x => x.IdStudent).ToList();
            if (entities != null)
                foreach (var entity in entities)
                {
                    var answers = entity.GroupBy(x => x.GetModule().Id).ToList();
                    foreach (var answer in answers)
                    {
                        DetailPreTestAnswerResponse model = new()
                        {
                            IdStudent = entity.Key,
                            IdModule = answer.Key,
                            PRTScore = answer.Where(s => s.Option.IsTrue).Count() * 10,
                            PRTDetail = answer.Select(x => new PreTestAnswerDetail(x)).ToList()
                        };
                        model.MapToModel(answer.FirstOrDefault().GetModule());
                        model.StudentInfo.MapToModel(answer.FirstOrDefault().Student);
                        models.Add(model);
                    }
                }

            return models as List<TModel>;
        }
        public override async Task<PreTestAnswer> Create<TModel>(TModel model)
        {
            List<PreTestAnswer> answers = model.MaptoListEntity<PreTestAnswer>();
            await _appDbContext.Set<PreTestAnswer>().AddRangeAsync(answers);
            await _appDbContext.SaveChangesAsync();
            return answers.FirstOrDefault();
        }
    }
}
