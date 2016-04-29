## OpenVPN on  Ubuntu

1. Prepare the configuration file, yosemitecloud.ovpn
3. Use different configuraton files for different VPN. Like yosemitecloud-ja.ovpn, yosemitecloud-sj.ovpn,...
3. Start the VPN, normally I will put this command to one script govpn.sh
```BASH
sudo openvpn --config ~/yosemitecloud.ovpn
````
4. Start VPN without input username/password
> create .login.conf file, the format will be
```
<USERNAME>
<PASSWORD>
```
