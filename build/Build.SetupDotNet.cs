using System.Collections.Generic;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.CI.GitHubActions.Configuration;
using Nuke.Common.Execution;
using Nuke.Common.Utilities;

public class GitHubActionsSetupDotNetStep : GitHubActionsStep
{
    public string[] Versions { get; set; }

    public override void Write(CustomFileWriter writer)
    {
        writer.WriteLine("- uses: actions/setup-dotnet@v4");

        using (writer.Indent())
        {
            writer.WriteLine("with:");
            using (writer.Indent())
            {
                writer.WriteLine("dotnet-version: |");
                using (writer.Indent())
                {
                    foreach (var version in Versions)
                    {
                        writer.WriteLine(version);
                    }
                }
            }
        }
    }
}

public class GithubActionsExtendedAttribute : GitHubActionsAttribute
{
    public string[] SetupDotNetVersions { get; set; } = [];
    public GithubActionsExtendedAttribute(string name, GitHubActionsImage image, params GitHubActionsImage[] images) : base(name, image, images)
    {
    }

    protected override GitHubActionsJob GetJobs(GitHubActionsImage image,
        IReadOnlyCollection<ExecutableTarget> relevantTargets)
    {
        var job = base.GetJobs(image, relevantTargets);

        var newSteps = new List<GitHubActionsStep>(job.Steps);
        newSteps.Insert(0, new GitHubActionsSetupDotNetStep(){Versions = SetupDotNetVersions});

        job.Steps = newSteps.ToArray();
        
        return job;
    }
}