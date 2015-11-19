Param(
  [Parameter(HelpMessage="Where are the source files for the application? (required)")]
  [string]$sourceFolder,
	
  [Parameter(Mandatory=$True,HelpMessage="Where is the folder where applications are installed to? (required)")]
  [string]$destinationFolder,
  
  [Parameter(Mandatory=$True,HelpMessage="Where is the folder where applications are backed up before being updated? (required)")]
  [string]$backupFolder,
	
  [Parameter(Mandatory=$True,HelpMessage="Where are the XDT transforms for the *.example.config files? (required)")]
  [string]$transformsFolder,

  [Parameter(Mandatory=$True,HelpMessage="What is the name of the IIS site where the application is being set up? (required)")]
  [string]$websiteName,
	
  [Parameter(Mandatory=$True,HelpMessage="Why is this change being made? (required)")]
  [string]$comment
)

########################################################################
### BOOTSTRAP. Copy this section into each application setup script  ###
###            to make sure required functions are available.        ###
########################################################################

$pathOfThisScript = Split-Path $MyInvocation.MyCommand.Path -Parent
$parentFolderOfThisScript = $pathOfThisScript | Split-Path -Parent
$scriptsProject = 'Escc.WebApplicationSetupScripts'
$functionsPath = "$pathOfThisScript\..\$scriptsProject\functions.ps1"
if (Test-Path $functionsPath) {
  Write-Host "Checking $scriptsProject is up-to-date"
  Push-Location "$pathOfThisScript\..\$scriptsProject"
  git pull origin master
  Pop-Location
  Write-Host
  .$functionsPath
} else {
  if ($env:GIT_ORIGIN_URL) {
    $repoUrl = $env:GIT_ORIGIN_URL -f $scriptsProject
    git clone $repoUrl "$pathOfThisScript\..\$scriptsProject"
  } 
  else 
  {
    Write-Warning '$scriptsProject project not found. Please set a GIT_ORIGIN_URL environment variable on your system so that it can be downloaded.
  
Example: C:\>set GIT_ORIGIN_URL=https://example-git-server.com/{0}"
  
{0} will be replaced with the name of the repository to download.'
    Exit
  }
}

########################################################################
### END BOOTSTRAP. #####################################################
########################################################################

$projectName = "Escc.SupportWithConfidence.Website" 
$sourceFolder = NormaliseFolderPath $sourceFolder "$PSScriptRoot\$projectName"
$destinationFolder = NormaliseFolderPath $destinationFolder
$backupFolder = NormaliseFolderPath $backupFolder
$transformsFolder = NormaliseFolderPath $transformsFolder

CheckApplicationExists $destinationFolder "Escc.EastSussexGovUK"
BackupApplication "$destinationFolder/$projectName" $backupFolder $comment

robocopy $sourceFolder "$destinationFolder/$projectName" /MIR /IF *.aspx *.ashx *.ascx *.dll *.jpg *.css *.js /XD aspnet_client obj Properties 

TransformConfig "$sourceFolder\web.example.config" "$destinationFolder\$projectName\web.config" "$transformsFolder\$projectName\web.release.config"

EnableDotNet40InIIS
CreateApplicationPool $projectName
CheckSiteExistsBeforeAddingApplication $websiteName
CreateVirtualDirectory $websiteName "socialcare" "$destinationFolder\_virtual"
CreateVirtualDirectory $websiteName "socialcare/athome" "$destinationFolder\_virtual"
CreateVirtualDirectory $websiteName "socialcare/athome/approvedproviders" "$destinationFolder\$projectName" true $projectName
CreateVirtualDirectory $websiteName "socialcare/athome/approvedproviders/masterpages" "$destinationFolder\Escc.EastSussexGovUK\masterpages" true

Write-Host
Write-Host "Done." -ForegroundColor "Green"