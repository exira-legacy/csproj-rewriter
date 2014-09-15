namespace CsProjRewriter
{
    using System;
    using System.Linq;
    using Microsoft.Build.Evaluation;

    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 1 || string.IsNullOrWhiteSpace(args[0]))
                Console.WriteLine("Specify csproj to rewrite");

            var collection = new ProjectCollection();
            var project = collection.LoadProject(args[0]);

            Cleanup(project, "Content");

            Cleanup(project, "None");
            
            project.Save();
        }

        private static void Cleanup(Project project, string itemTypeToCleanup)
        {
            var items = project.Items.Where(x => x.ItemType == itemTypeToCleanup).ToList();
            project.RemoveItems(items);

            var sortedItems = items.OrderBy(x => x.UnevaluatedInclude);

            foreach (var item in sortedItems)
                project.AddItemFast(itemTypeToCleanup, item.UnevaluatedInclude);
        }
    }
}
