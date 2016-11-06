properties([
	parameters([
		string(defaultValue: '', description: '', name: 'ReleaseVersion'), 
		string(defaultValue: '', description: '', name: 'NextVersion'), 
		string(defaultValue: false, description: '', name: 'IsRelease')
	]), 
	pipelineTriggers([])
])

properties([buildDiscarder(logRotator(artifactDaysToKeepStr: '', artifactNumToKeepStr: '', daysToKeepStr: '', numToKeepStr: '5')), pipelineTriggers([])])

nugetPipeline {
  project = "dotnet-version"
  notificationRecipients = "paul.trampert@gmail.com"
}
