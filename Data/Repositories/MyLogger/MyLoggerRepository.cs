namespace InboundApi.Data.Repositories
{
    public class MyLoggerRepository : GenericRepository<MyLogger>, IMyLoggerRepository
    {
        public MyLoggerRepository(AppDbContext context) : base(context) { }

    }
}
