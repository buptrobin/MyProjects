$servicelist = @("AIJobService", "SSWinService","SegmentStudioWebService","SegmentStudioWebSite","SSModelingService")
foreach ($a in $servicelist){
	$app = Get-WmiObject -Class Win32_Product -Filter "Name = $a"
	$app
}