Set-StrictMode -Version 3

$currentDir = split-path $SCRIPT:MyInvocation.MyCommand.Path -parent

. (Join-Path $currentDir "template-mechanism.ps1")

function Get-NotEmpty($a, $b) 
{ 
    if ([string]::IsNullOrWhiteSpace($a)) 
    { 
        $b 
    } else 
    { 
        $a 
    }
}

function Get-UsersGroupSID()
{
    try
    {
        $objUsersGroup = New-Object System.Security.Principal.NTAccount('Users')
        $strSID = $objUsersGroup.Translate([System.Security.Principal.SecurityIdentifier])
        $strSID.Value
    }
    catch
    {
        Write-Host "Could not get SID for the local 'User' group. Aborting." -ForegroundColor Red
        exit 1
    }
}

Export-ModuleMember Write-Template
Export-ModuleMember Run-Template
Export-ModuleMember Get-NotEmpty
Export-ModuleMember Get-UsersGroupSID