echo $_
$begin = $args[0]
$end = $args[1]

$i=1
for(; $i -le 50; $i++){
	$s = '<task market="AU" owner="Ed" segmentname="'+$i+'" ProductionStatus="Live" SegmentAction="New"/>' 
	echo $s
}

for(; $i -le 250; $i++){
	$s = '<task market="US" owner="Ed" segmentname="'+$i+'" ProductionStatus="Live" SegmentAction="New"/>'
	echo $s
}

#for($i=1; $i -le $args[0]; $i++){
#	$s = '<task market="AU" owner="Ed" segmentname="'+$i+'" ProductionStatus="Live" SegmentAction="New"/>'
#	echo $s
#}

echo $begin
echo asdf
