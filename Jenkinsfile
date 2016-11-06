properties([buildDiscarder(logRotator(artifactDaysToKeepStr: '', artifactNumToKeepStr: '', daysToKeepStr: '', numToKeepStr: '5')), pipelineTriggers([])])

nugetPipeline {
  project = "dotnet-version"
  notificationRecipients = "paul.trampert@gmail.com"
}
