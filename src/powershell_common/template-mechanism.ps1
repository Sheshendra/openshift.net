# Using a simple templating mechanism for configuring mcollective files

function Write-Template {
    param(
        [string]$inFile,
        [string]$outFile,
        [Scriptblock]$scriptBlock
    )
    
    
    $fullScript = [ScriptBlock]::Create($scriptBlock.ToString() + "
`$content = [IO.File]::ReadAllText( `$inFile )
Invoke-Expression `"@```"``r``n`$content``r``n```"@`" | Out-File $outFile -Encoding ascii
")

    & $fullScript
}