// JScript source code
// Get a user's SID from the domain
// Usage: cscript.exe GetSID.js userName, DomainName

if (WScript.Arguments.Length != 2)
{
    WScript.Echo("Missing argument, Usage: cscript.exe GetSID.js <userName> <DomainName>.");
    WScript.Quit(1);
}

var userName = WScript.Arguments(0);

var domain = WScript.Arguments(1);
var strComputer = ".";
try
{
    var objWMIService = GetObject("winmgmts:\\\\" +strComputer  +"\\root\\cimv2")
    var objAccount = objWMIService.Get("Win32_UserAccount.Name='" +userName +  "',Domain='" + domain +"'")
    WScript.Echo(objAccount.SID);
}
catch(ex)
{
    WScript.Quit(1);
}
