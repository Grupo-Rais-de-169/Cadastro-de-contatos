namespace TechChallenge.DAO.Api.Configuration
{
    public class MassTransitConfig
    {
        public string CreateQueue { get; set; } = default!;
        public string UpdateQueue { get; set; } = default!;
        public string DeleteQueue { get; set; } = default!;
        public string Server { get; set; } = default!;
        public string User { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
