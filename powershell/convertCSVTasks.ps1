$inputfile = "E:\ms\bt\SS\document\09Florence\MayDeploymentPlan.csv"
$outputfile = "E:\ms\bt\SS\document\09Florence\converted.txt"
Clear-Content $outputfile

$alltasks = Get-Content $inputfile
foreach($task in $alltasks)
{	
	$a = $task.split(",")
	$mkt = $a[0]
	$segmentname = $a[1]
	$axname = $a[2]
	$status = $a[3]
	$owner = $a[4]
	$action = $a[5]
	
	$s = '<task market="'+$mkt+'" owner="'+$owner+'" segmentname="'+$segmentname+'" ProductionStatus="'+$status+'" SegmentAction="'+$action+'"/>'
	#echo $s
	Add-Content $outputfile $s
}



