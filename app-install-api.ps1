Param(
  [Parameter(HelpMessage="Where are the source files for the application?")]
  [string]$sourceFolder,
	
  [Parameter(Mandatory=$True,HelpMessage="Where is the folder where applications are installed to? (required)")]
  [string]$destinationFolder,
  
  [Parameter(HelpMessage="Where is the folder where applications are backed up before being updated?")]
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

$destinationFolder = NormaliseFolderPath $destinationFolder
$backupFolder = NormaliseFolderPath $backupFolder
if (!$backupFolder) { $backupFolder = "$destinationFolder\backups" }
$backupFolder = "$backupFolder\$websiteName"
$destinationFolder = "$destinationFolder\$websiteName"
$transformsFolder = NormaliseFolderPath $transformsFolder

EnableDotNet40InIIS

# Install the web service
$apiProjectName = "Escc.SupportWithConfidence.WebApi" 
$apiSourceFolder = NormaliseFolderPath $sourceFolder "$PSScriptRoot\$apiProjectName"

BackupApplication "$destinationFolder/$apiProjectName" $backupFolder $comment

# Copy files
robocopy $apiSourceFolder "$destinationFolder/$apiProjectName" /MIR /IF *.asax *.dll *.pdb csc.* csi.* /XD App_Data App_Start Controllers obj Properties "Connected Services" aspnet_client

TransformConfig "$apiSourceFolder\web.example.config" "$destinationFolder\$apiProjectName\web.config" "$transformsFolder\$apiProjectName\web.config.xdt"
if (Test-Path "$transformsFolder\$apiProjectName\web.config.$websiteName.xdt") {
	# Transform to temp file to avoid file locking problem
	TransformConfig "$destinationFolder\$apiProjectName\web.config" "$destinationFolder\$apiProjectName\web.temp.config" "$transformsFolder\$apiProjectName\web.config.$websiteName.xdt"
	copy "$destinationFolder\$apiProjectName\web.temp.config" "$destinationFolder\$apiProjectName\web.config"
	del "$destinationFolder\$apiProjectName\web.temp.config"
}

CreateApplicationPool "$apiProjectName-$websiteName"
CheckSiteExistsBeforeAddingApplication $websiteName
if ((Get-Item "IIS:\Sites\$websiteName\$apiProjectName").ApplicationPool -eq $null) {
	Write-Host "Setting $apiProjectName directory to be an IIS application"
	ConvertTo-WebApplication "IIS:\Sites\$websiteName\$apiProjectName"
} else {
	Write-Host "$apiProjectName is already an IIS application"
}
Write-Host "Setting application pool to $apiProjectName-$websiteName"
Set-ItemProperty "IIS:\Sites\$websiteName\$apiProjectName" -Name applicationPool -Value "$apiProjectName-$websiteName"
DisableAnonymousAuthentication $websiteName "$apiProjectName"
EnableWindowsAuthentication $websiteName "$apiProjectName"

# Give application pool account write access to the parent folder (site root) because it won't work without it. 
Write-Host "Granting Modify access to the application pool account"
$acl = Get-Acl $destinationFolder
$rule = New-Object System.Security.AccessControl.FileSystemAccessRule("IIS AppPool\$apiProjectName-$websiteName", "Modify", "ContainerInherit,ObjectInherit", "None", "Allow")
$acl.SetAccessRule($rule)
Set-Acl $destinationFolder $acl

Write-Host
Write-Host "Done." -ForegroundColor "Green"