#.\init.ps1
#.\lib.ps1

#initEnv_SI

function CheckService()
{
	ConfirmServiceRunning $Machine_SSMT SSWinService
	ConfirmServiceRunning $Machine_SSUI SSWebService
	ConfirmServiceRunning $Machine_SSMT AIJobService
}

function CheckDB()
{

}

#CheckService

	&{
	Write-Host "111"
	throw( New-Object NullReferenceException)
	}
	trap[Exception]{
		Write-Host "trapped"
	}