using System.Text.Json;
using users_service.Src.Models;

namespace users_service.Src.Data
{
    public class Seed
    {
        public static void SeedData(DataContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            CallEachSeeder(context, options);
        }

        /// <summary>
        /// Centralize the call to each seeder method
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="options">Options to deserialize json</param>
        public static void CallEachSeeder(DataContext context, JsonSerializerOptions options)
        {
            SeedFirstOrderTables(context, options);
        }

        /// <summary>
        /// Seed the database with the tables that don't depend on other tables.
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="options">Options to deserialize json</param>
        private static void SeedFirstOrderTables(DataContext context, JsonSerializerOptions options)
        {
            SeedSubjects(context, options);
            SeedCareers(context, options);
        }

        /// <summary>
        /// Seed the database with the subjects in the json file and save changes if the database is empty.
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="options">Options to deserialize json</param>
        private static void SeedSubjects(DataContext context, JsonSerializerOptions options)
        {
            var result = context.Subjects?.Any();
            if (result is true or null) return;

            var path = "Src/Data/DataSeeders/SubjectsData.json";
            var subjectsData = File.ReadAllText(path);
            var subjectsList = JsonSerializer.Deserialize<List<Subject>>(subjectsData, options) ??
                throw new Exception("SubjectsData.json is empty");
            // Normalize the name, code of the subjects
            subjectsList.ForEach(s =>
            {
                s.Code = s.Code.ToLower();
            });

            context.Subjects?.AddRange(subjectsList);
            context.SaveChanges();
        }

        /// <summary>
        /// Seed the database with the careers in the json file and save changes if the database is empty.
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="options">Options to deserialize json</param>
        private static void SeedCareers(DataContext context, JsonSerializerOptions options)
        {
            var result = context.Careers?.Any();
            if (result is true or null) return;
            var path = "Src/Data/DataSeeders/CareersData.json";
            var careersData = File.ReadAllText(path);
            var careersList = JsonSerializer.Deserialize<List<Career>>(careersData, options) ??
                throw new Exception("CareersData.json is empty");
            // Normalize the name and code of the careers
            careersList.ForEach(s =>
            {
                s.Name = s.Name.ToLower();
            });

            context.Careers?.AddRange(careersList);
            context.SaveChanges();
        }
    }
}