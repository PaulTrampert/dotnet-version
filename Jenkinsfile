properties([
	parameters([
		string(defaultValue: '', description: '', name: 'ReleaseVersion'), 
		string(defaultValue: '', description: '', name: 'NextVersion'), 
		string(defaultValue: false, description: '', name: 'IsRelease')
	]), 
	pipelineTriggers([])
])

properties([buildDiscarder(logRotator(artifactDaysToKeepStr: '', artifactNumToKeepStr: '', daysToKeepStr: '', numToKeepStr: '5')), pipelineTriggers([])])

env.ReleaseVersion = params.ReleaseVersion
env.NextVersion = params.NextVersion

nugetPipeline {
  project = "dotnet-version"
  notificationRecipients = "paul.trampert@gmail.com"
  isRelease = params.IsRelease
}
