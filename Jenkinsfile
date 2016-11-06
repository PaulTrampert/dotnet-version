properties([buildDiscarder(logRotator(artifactDaysToKeepStr: '', artifactNumToKeepStr: '', daysToKeepStr: '', numToKeepStr: '5')), pipelineTriggers([])])

nugetPipeline {
  gitRepoUrl = "https://github.com/PaulTrampert/dotnet-version"
  project = "dotnet-version"
  notificationRecipients = "paul.trampert@gmail.com"
}
