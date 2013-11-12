param (
    $cygwinDir = $( Read-Host "Path to setup directory (c:\cygwin)" )
)

$currentDir = split-path $SCRIPT:MyInvocation.MyCommand.Path -parent
Import-Module (Join-Path $currentDir '..\..\powershell_common\openshift-common.psm1')

$cygwinDir = Get-NotEmpty $targetDirectory "c:\cygwin"

$usersGroupSID = Get-UsersGroupSID

Write-Host 'Using setup dir: ' -NoNewline
Write-Host $cygwinDir -ForegroundColor Yellow

$cygwinSetupProgramURL = 'http://cygwin.com/setup-x86_64.exe'
$setupPackage = Join-Path $env:TEMP "setup-x86_64.exe"

if ((Test-Path $setupPackage) -eq $true)
{
    rm $setupPackage -Force > $null
}

Write-Host "Downloading the setup program from here: " -NoNewline
Write-Host $cygwinSetupProgramURL -ForegroundColor Yellow

Invoke-WebRequest $cygwinSetupProgramURL -OutFile $setupPackage

if ((Test-Path $cygwinDir) -eq $true)
{
    rmdir $cygwinDir -Recurse -Force > $null
}

mkdir $cygwinDir > $null

$packageDir = Join-Path $cygwinDir "packages"
$installationDir = Join-Path $cygwinDir "installation"

mkdir $packageDir > $null
mkdir $installationDir > $null


if ((Test-Path $setupPackage) -ne $true)
{
    Write-Host "Can't find Cygwin setup program. Aborting." -ForegroundColor red
    exit 1
}

$packages = "openssh", "cygrunsrv", "git"

$site = "http://mirrors.kernel.org/sourceware/cygwin/"

$packagesArg = [string]::Join(',', $packages)

$arguments = "-d -n -N -q -r -s `"$site`" -a x86_64 -l `"$packageDir`" -R `"$installationDir`" -P `"$packagesArg`""

Write-Host "Setting up cygwin with the following arguments: " -NoNewline
Write-Host $arguments -ForegroundColor Yellow

Start-Process $setupPackage $arguments -Wait

Write-Host "Cygwin setup complete." -ForegroundColor Green

Write-Host "Erasing 'passwd' file."

$passwdFile = Join-Path $cygwinDir 'installation\etc\passwd'
echo $null > $passwdFile

$groupFile = Join-Path $cygwinDir 'installation\etc\group'
Write-Host "Setting up groups file."
$gid = $usersGroupSID.Split('-')[-1]
echo "Users:${usersGroupSID}:" > $groupFile

