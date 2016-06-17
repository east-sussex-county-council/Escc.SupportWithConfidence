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

$websiteProject = "Escc.SupportWithConfidence.Website"
$adminProject = "Escc.SupportWithConfidence.Admin"
$etlProject = "Escc.SupportWithConfidence.ETL"
$apiProject = "Escc.SupportWithConfidence.WebApi"

DownloadProjectIfMissing $parentFolderOfThisScript "Escc.EastSussexGovUK"
DownloadProjectIfMissing $parentFolderOfThisScript "Escc.Data.Web"

# Front-end website
EnableDotNet40InIIS
CreateApplicationPool $websiteProject
CreateWebsite $websiteProject "$pathOfThisScript\$websiteProject"
CreateHTTPSBinding $websiteProject "localhost"
CreateVirtualDirectory $websiteProject "Escc.EastSussexGovUK" "$parentFolderOfThisScript\Escc.EastSussexGovUK" true
CreateVirtualDirectory $websiteProject "masterpages" "$parentFolderOfThisScript\Escc.EastSussexGovUK\masterpages" true
CopyConfig "$websiteProject\Web.example.config" "$websiteProject\web.config"

# Admin website
CreateApplicationPool $adminProject
CreateWebsite $adminProject "$pathOfThisScript\$adminProject"
CreateHTTPSBinding $adminProject "localhost"
CreateVirtualDirectory $adminProject "Escc.EastSussexGovUK" "$parentFolderOfThisScript\Escc.EastSussexGovUK" true
CreateVirtualDirectory $adminProject "masterpages" "$parentFolderOfThisScript\Escc.EastSussexGovUK\masterpages" true
DisableAnonymousAuthentication $adminProject
EnableWindowsAuthentication $adminProject
CopyConfig "$adminProject\Web.example.config" "$adminProject\web.config"

# API
CreateApplicationPool $apiProject
CreateWebsite $apiProject "$pathOfThisScript\$apiProject"
CreateHTTPBinding $apiProject
CopyConfig "$apiProject\Web.example.config" "$apiProject\web.config"

# Configure ETL admin tool
CopyConfig "$etlProject\app.example.config" "$etlProject\app.config"

Write-Host
Write-Host "Done." -ForegroundColor "Green"
Write-Host
Write-Host "Now replace the sample values in 'app.config' and 'web.config' with ones appropriate to your setup, and build the solution." -ForegroundColor "Green"
Write-Host "You will also need to enable Windows authentication and remove anonymous authentication for the Web API site in IIS." -ForegroundColor "Green"