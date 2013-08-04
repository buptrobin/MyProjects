echo $_
$datebegin = "20090213"
$dateend = "20090329"

$d1 = [datetime] ($datebegin.substring(0,4)+"/"+$datebegin.substring(4,2)+"/"+$datebegin.substring(6,2))
$d2 = [datetime] ($dateend.substring(0,4)+"/"+$dateend.substring(4,2)+"/"+$dateend.substring(6,2))
$a =""
do {
	$s = (get-date $d1 -f "yyyyMMdd")
	$a=$a+" "+$s
	$d1 = ($d1).AddDays(1)
}
until ($d1 -gt $d2)
echo $a
