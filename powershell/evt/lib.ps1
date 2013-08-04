###########################################################################
##   lib.ps1
##   The lib for the EVT       
##
###########################################################################

function GLOBAL:gCheckServiceState()
{
	param(
		[string]$computerName,
		[string]$serviceName
	)
	
	$ret = [bool]"Fasle"
	Get-Service|Where-Object {$_.Status -eq "Running"}
	$a = Get-Service -ComputerName $computerName -Name $serviceName
	if($a.Status -eq "Running") {
		[bool]$ret = [bool]"True"
	}
	$ret
}

function GLOBAL:gCheckServiceExist()
{
	param(
		[string]$computerName,
		[string]$serviceName
	)
	
	#$ret = [bool]"Fasle"
	Get-Service -ComputerName $computerName|Where-Object {$_.Name -eq $serviceName}
}


#--------------------------------------------------------------
# Check the file specified whether exist
# Returen Value: True: exist
#                False: not exist
#--------------------------------------------------------------
function GLOBAL:gCheckFileExist()
{
	param(
		[string]$filePath
	)

	Test-Path $filePath
}

#--------------------------------------------------------------
# Check whether the Login in DB logins
# Returen Value: True: exist
#                False: not exist
#--------------------------------------------------------------
function GLOBAL:gCheckLoginExist()
{
	param(
		[string] $computerName,
		[string] $loginName
		)
	
	$loginPath = dir SQLSERVER:\SQL\$computerName\DEFAULT\logins |Where-Object {$_.Name -eq $loginName}
	
	$ret = False
	
	if($loginName) {$ret=True}
	
	$ret
}

#--------------------------------------------------------------
# Check whether the Login in DB logins
# Returen Value: True: exist
#                False: not exist
#--------------------------------------------------------------
function GLOBAL:gCheckDBExist()
{
	param(
		[string] $computerName,
		[string] $DBName
		)
	
	$dbPath = dir SQLSERVER:\SQL\$computerName\DEFAULT\DATABASE |Where-Object {$_.Name -eq $loginName}

	$ret = False
	
	if($dbPath) {$ret=True}
	
	$ret
}

#--------------------------------------------------------------
# Check whether the Login in DB logins
# Returen Value: True: exist
#                False: not exist
#--------------------------------------------------------------
function GLOBAL:gCheckDBLoginExist()
{
	param([string] $computerName,[string] $DBName)
	
	$ret = dir SQLSERVER:\SQL\$computerName\DEFAULT\logins |Where-Object {$_.Name -eq $loginName}

	$ret
}
#--------------------------------------------------------------
# Check whether the account in the security group
# Returen Value: True: exist
#                False: not exist
#--------------------------------------------------------------
function GLOBAL:gCheckDBExist()
{
	param([string] $computerName,[string] $DBName)
	
	$ret = dir SQLSERVER:\SQL\$computerName\DEFAULT\DATABASES |Where-Object {$_.Name -eq $DBName}
	
	$ret
}

#--------------------------------------------------------------
# Check whether the account in the security group
# Returen Value: True: exist
#                False: not exist
#--------------------------------------------------------------
function GLOBAL:gCheckAccountInSecurityGroup()
{
	param(
		[string] $account,
		[string] $securityGroup
		)
	$ret=$False
	$output=net group $securityGroup /domain
	foreach($s in $output){
		if($s -ne $null){
			$c=$s.IndexOf("$account")
			if($c -ge 0) {
				$ret=$true
				break
			}
		}
	}
	
	$ret
}
function GLOBAL:gGetMachineAccount(){
	param([string]$computername)
	$dm = $env:USERDOMAIN
	$ret="$dm\$computername$"
	$ret
}

function GLOBAL:gGetFileFullPath(){
	param([string]$machine,[string]$path)
	
	$ret="\\$machine$path"
	$ret
}

#---------------------------------------------------------------
# To do check with output
#---------------------------------------------------------------
function GLOBAL:ConfirmServiceRunning(){
	param([string]$computerName, [string]$serviceName)	
	
	Write-Host -NoNewline -ForegroundColor White "Check Service $serviceName is running on $computerName..."
	if(gCheckServiceExist $computerName $serviceName){	
		$state = gCheckServiceState $computerName $serviceName
		if($state){
			Write-Host -ForegroundColor Green "Pass"
		}
		else{
			Write-Host -ForegroundColor Red "Not Running"
		}
	}
	else {
		Write-Host -ForegroundColor Red "No service $serviceName on $computerName"
	}
}

function GLOBAL:ConfirmDBLoginExist(){
	param([string] $computerName,[string] $loginName, [string]$dbname)
	
	Write-Host -NoNewline -ForegroundColor White "Check login $loginName on $dbname in $computerName ..."
	
	if(gCheckDBExist $computerName $dbname){
		$login = gCheckDBLoginExist $computerName $loginName
		if($login){
			$map = $login.EnumDatabaseMappings()
			if($map|where {$_.DBName -eq $DBName}){
				Write-Host -ForegroundColor Green "Pass"
			}
			else{
				Write-Host -ForegroundColor Red  "Database login $loginName is exist in $computerName, but cannot access $dbname"
			}			
		}
		else{
			Write-Host -ForegroundColor Red  "Login $loginName is not exist in $dbname on $computerName"
		}
	}
	else{
		Write-Host -ForegroundColor Red  "Database $dbname is not exist in $computerName"
	}
}

function GLOBAL:ConfirmDBExist(){
	param([string] $computerName,[string] $dbname)
	
	Write-Host -NoNewline -ForegroundColor White "Check Database $dbname existence on $computerName..."
	
	$state = gCheckDBExist $computerName $dbname
	if($state){
		Write-Host -ForegroundColor Green "Pass"
	}
	else{
		Write-Host -ForegroundColor Red  "Not exist"
	}
}

function GLOBAL:ConfirmAccountInSecuritygroup(){
	param([string] $account,[string] $group)
	
	Write-Host -NoNewline -ForegroundColor White "Check Account $account existence in security group $group..."
	
	$a=$group.IndexOf("\")
	$groupname=$group
	if($a -ge 0){
		$groupname=$group.substring($a+1)
	}
	
	$a=$account.IndexOf("\")
	$acctname=$account
	if($a -ge 0){
		$acctname=$account.substring($a+1)
	}
	
	$state = gCheckAccountInSecurityGroup $acctname $groupname
	if($state){
		Write-Host -ForegroundColor Green "Pass"
	}
	else{
		Write-Host -ForegroundColor Red  "Not exist"
	}
}

function GLOBAL:ConfirmFileExist(){
	param([string] $filepath)
	Write-Host -NoNewline -ForegroundColor White "Check File existence on $filepath..."
	$state = gCheckFileExist $filepath
	if($state){
		Write-Host -ForegroundColor Green "Pass"
	}
	else{
		Write-Host -ForegroundColor Red  "Not exist"
	}
}