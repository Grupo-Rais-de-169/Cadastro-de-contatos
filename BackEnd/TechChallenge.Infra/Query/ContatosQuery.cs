namespace TechChallenge.Infra.Query
{
    public static class ContatosQuery
    {
        public static string ListDDD(int? id = null)
        {
            string query = @"SELECT * FROM ""CodigosDeArea"" a";

            if (id.HasValue)
            {
                query += @" WHERE a.Id = @Id";
            }
            return query;
        }

        public static string GetContatoByDDD(int id)
        {
            string query = @"SELECT ""Nome"", ""Email"", ""Telefone"", ""IdDDD"" DDD FROM ""Contatos""  where ""IdDDD"" = @Id";
            return query;
        }
    }
}
