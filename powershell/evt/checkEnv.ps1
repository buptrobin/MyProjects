.\init.ps1
.\lib.ps1

initEnv_SI

function CheckService()
{
	Write-Host -ForegroundColor Yellow "=====Start to check service running state====="
	ConfirmServiceRunning $Machine_SSMT "SSWinService"
	#ConfirmServiceRunning $Machine_SSUI "SSWebService"
	ConfirmServiceRunning $Machine_SSMT "AIJobService"
	ConfirmServiceRunning $Machine_SSMT "SSModelingService"
	ConfirmServiceRunning $Machine_RuleFastScoring "BT SeMS Fast Scoring Service For SS"
	ConfirmServiceRunning $Machine_TaxonomyFastScoring "BT SeMS Fast Scoring Service For SS"
	
	ConfirmServiceRunning $Machine_MDSMT "MDSService"
	
	Write-Host
}

function CheckDB()
{
	Write-Host -ForegroundColor Yellow "=====Start to check database existing====="
	ConfirmDBExist $Machine_SSDB "SegmentStudioDB"
	ConfirmDBExist $Machine_SSDB "BTSRDB"
	ConfirmDBExist $Machine_MDSDB "AIMetadataDB"
	ConfirmDBExist $Machine_DB_AtlasDimension "Atlas_Dim"
	
	Write-Host
}

function CheckDBExist()
{
	Write-Host -ForegroundColor Yellow "=====Start to check database existing====="
	ConfirmDBExist $Machine_SSDB "SegmentStudioDB"
	ConfirmDBExist $Machine_SSDB "BTSRDB"
	ConfirmDBExist $Machine_MDSDB "AIMetadataDB"
	ConfirmDBExist $Machine_DB_AtlasDimension "Atlas_Dim"
	Write-Host
}

function CheckDBLogins()
{
	Write-Host -ForegroundColor Yellow "=====Start to check database logins====="
	ConfirmDBLoginExist $Machine_SSDB (gGetMachineAccount $Machine_SSMT) "SegmentStudioDB"
	ConfirmDBLoginExist $Machine_MDSDB (gGetMachineAccount $Machine_SSMT) "AIMetadataDB"	
	ConfirmDBLoginExist $Machine_DB_AtlasDimension (gGetMachineAccount $Machine_SSMT) "Atlas_Dim"

	ConfirmDBLoginExist $Machine_SSDB (gGetMachineAccount $Machine_SSUI) "SegmentStudioDB"
	
	ConfirmDBLoginExist $Machine_SSDB $SSACCOUNT "SegmentStudioDB"
	ConfirmDBLoginExist $Machine_MDSDB $SSACCOUNT "AIMetadataDB"	
	ConfirmDBLoginExist $Machine_DB_AtlasDimension $SSACCOUNT "Atlas_Dim"	
	Write-Host	
}

function CheckAccountSecurityGroup()
{
	$a=$SSACCOUNT.IndexOf("\")
	$ssname=$SSACCOUNT
	if($a -ge 0){
		$ssname=$SSACCOUNT.substring($a+1)
	}
	Write-Host -ForegroundColor Yellow "=====Start to check account of security group====="
	#mds service security group
	
	ConfirmAccountInSecuritygroup $ssname $SG_MDSMT
	ConfirmAccountInSecuritygroup $Machine_SSUI $SG_MDSMT
	
	#cosmos access security group
	ConfirmAccountInSecuritygroup $ssname $SG_COSMOS
	
	#mds db security group
	Write-Host
}

function CheckFilesGenerated()
{
	$a=$SSACCOUNT.IndexOf("\")
	$ssname=$SSACCOUNT
	if($a -ge 0){
		$ssname=$SSACCOUNT.substring($a+1)
	}
	Write-Host -ForegroundColor Yellow "=====Start to check the files generated are exist====="
	ConfirmFileExist (gGetFileFullPath $Machine_SSMT "\C$\$FILE_SSGTM_PWD")
	ConfirmFileExist (gGetFileFullPath $Machine_SSMT "\C$\$FILE_SSGTMUser")
	ConfirmFileExist (gGetFileFullPath $Machine_SSMT "\C$\$FILE_SSMDS_CUSTOMID")
	ConfirmFileExist (gGetFileFullPath $Machine_SSMT "\C$\$FILE_SSMDS_PWD")
	ConfirmFileExist (gGetFileFullPath $Machine_SSMT "\C$\$FILE_SSMDS_USER")
	
	ConfirmFileExist (gGetFileFullPath $Machine_SSMT "\D$\$FILE_ModelMDS_CUSTOMID")
	ConfirmFileExist (gGetFileFullPath $Machine_SSMT "\D$\$FILE_ModelMDS_PWD")
	ConfirmFileExist (gGetFileFullPath $Machine_SSMT "\D$\$FILE_ModelMDS")
	
	ConfirmFileExist (gGetFileFullPath $Machine_SSMT "\C$\Users\$ssname\$FILE_AIJob_MDS_CUSTOMID")
	ConfirmFileExist (gGetFileFullPath $Machine_SSMT "\C$\Users\$ssname\$FILE_AIJob_MDS_PWD")
	ConfirmFileExist (gGetFileFullPath $Machine_SSMT "\C$\Users\$ssname\$FILE_AIJob_MDS_User")
	Write-Host
}


CheckService
CheckDBExist
CheckDBLogins
CheckAccountSecurityGroup
CheckFilesGenerated