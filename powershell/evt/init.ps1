###########################################################################
##
## init.ps1
## To initialize the environment
##
###########################################################################

# By default is one-box
$LocalName = hostname
$GLOBAL:Machine_SSUI = $LocalName
$GLOBAL:Machine_SSMT = $LocalName
$GLOBAL:Machine_SSDB = $LocalName
$GLOBAL:Machine_RuleFastScoring = $LocalName
$GLOBAL:Machine_TaxonomyFastScoring = $LocalName
$GLOBAL:Machine_MDSMT = $LocalName
$GLOBAL:Machine_MDSDB = $LocalName
$GLOBAL:Machine_DB_AtlasDimension = "CO2APSE071"
$GLOBAL:SSACCOUNT = "fareast\idsswtt"
$GLOBAL:SG_Access_MDS = "AIS-AudIntClients-SI"
$GLOBAL:SG_Access_COSMOS = "sch-audint-cosmos"
$GLOBAL:DOMAIN=$env:USERDOMAIN

$GLOBAL:SG_COSMOS = "sch-audint-cosmos"
$GLOBAL:SG_MDSMT = "AIS-MDS-CLIENTS"
$GLOBAL:SG_MDSDB = "AIS-MDS-DBClients"
 
$GLOBAL:FILE_SSGTM_PWD = 'SS\SysSvc\SSWinService\encodeUtility\secure\$SegmentStudio$GTM.pwd'
$GLOBAL:FILE_SSGTMUser = 'SS\SysSvc\SSWinService\encodeUtility\secure\$SegmentStudio$GTM.user'
$GLOBAL:FILE_SSMDS_CUSTOMID = 'SS\SysSvc\SSWinService\encodeUtility\secure\$SegmentStudio$MDS.customId'
$GLOBAL:FILE_SSMDS_PWD = 'SS\SysSvc\SSWinService\encodeUtility\secure\$SegmentStudio$MDS.pwd'
$GLOBAL:FILE_SSMDS_USER = 'SS\SysSvc\SSWinService\encodeUtility\secure\$SegmentStudio$MDS.user'

$GLOBAL:FILE_ModelMDS_CUSTOMID = 'BTSeMS\SSModelingService\encodeUtility\secure\$SegmentStudio$MDSSuper.customId'
$GLOBAL:FILE_ModelMDS_PWD = 'BTSeMS\SSModelingService\encodeUtility\secure\$SegmentStudio$MDSSuper.pwd'
$GLOBAL:FILE_ModelMDS = 'BTSeMS\SSModelingService\encodeUtility\secure\$SegmentStudio$MDSSuper.user'

$GLOBAL:FILE_AIJob_MDS_CUSTOMID = 'AppData\Roaming\encodeUtility\secure\$SegmentStudio$MDSSuper.customId'
$GLOBAL:FILE_AIJob_MDS_PWD = 'AppData\Roaming\encodeUtility\secure\$SegmentStudio$MDSSuper.pwd'
$GLOBAL:FILE_AIJob_MDS_User = 'AppData\Roaming\encodeUtility\secure\$SegmentStudio$MDSSuper.user'


function GLOBAL:initEnv_SI(){
	$GLOBAL:Machine_SSUI = "BY2MTZC169"
	$GLOBAL:Machine_SSMT = "BY2MTZC168"
	$GLOBAL:Machine_SSDB = "BY2MTZC643"
	$GLOBAL:Machine_RuleFastScoring = "BY2MTZC641"
	$GLOBAL:Machine_TaxonomyFastScoring = "BY2MTZC642"
	$GLOBAL:Machine_MDSMT = "BY2MTZC619"
	$GLOBAL:Machine_MDSDB = "BY2MTZC620"
	$GLOBAL:Machine_DB_AtlasDimension = "CO2APSE071"

#	$GLOBAL:SG_Access_MDS = "AIS-AudIntClients-SI"
#	$GLOBAL:SG_Access_COSMOS = "sch-audint-cosmos"
	
	$GLOBAL:SSACCOUNT = "PHX\_SEMS-ACCOUNT"
	
	$SG_COSMOS = "PHX\sch-audint-cosmos"
	$SG_MDSMT = "PHX\AIS-AudIntClients-SI"
	$SG_MDSDB = "PHX\AIS-MDS-DBClients"
}

function GLOBAL:initEnv_PROD(){
	$GLOBAL:Machine_SSUI = "CO2APSE003"
	$GLOBAL:Machine_SSMT = "CO2APSE045"
	$GLOBAL:Machine_SSDB = "CO2AISSeMSQL01"
	$GLOBAL:Machine_RuleFastScoring = "CO2ADCD002"
	$GLOBAL:Machine_TaxonomyFastScoring = "CO2ADCD002"
	$GLOBAL:Machine_MDSMT = "10.2.7.37"
	$GLOBAL:Machine_MDSDB = "BY2AISSQL02"
	$GLOBAL:Machine_DB_AtlasDimension = "CO2AISBTSQL01"	

#	$GLOBAL:SG_Access_MDS = "AIS-AudIntClients-SI"
#	$GLOBAL:SG_Access_COSMOS = "phx\sch-aiauthoringprod-cosmos"
	
	$GLOBAL:SSACCOUNT = "phx\_AIS-SSeMSPd_exe"
	
	$SG_COSMOS = "phx\sch-aiauthoringprod-cosmos"
	$SG_MDSMT = "PHX\PHX\AIS-MDS-CLIENTS"
	$SG_MDSDB = "PHX\AIS-MDS-DBClients"
}

.\initSQL.ps1
