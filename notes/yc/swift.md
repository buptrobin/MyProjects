
## Ad Creative Store in SWIFT

### SWIFT
+ get the token and storage URL (done)
+ set permission to public read (done)
### Network
+ The first entry point is keepalived.  assuming we can use a different port number for the creative we can go directly from keepalived to swift.  Or keepalived to varnish to swift.
### ui/frontend
+ upload ad Creative
+ save the file to SWIFT,
  + set the content_url to the file name
  + Generate the file name, should make sure it is unique
  + check whether unique, if, then save, or will generate again
  + the content saved should include container and file name r:  <container>/<filename>
### RTS
+ in the response, fetch creative metadata, and get the public URL from swift
+ send back the URL to ADX
+ config to store the username and password, or token
+ POM.xml maybe need add dependency of JOSS
```XML
<dependency>
    <groupId>org.javaswift</groupId>
    <artifactId>joss</artifactId>
    <version>0.9.10</version>
</dependency>
```
### DB
+ Add new storage type, AdStorageType (done)
### Performance test (Nia)
The other task is testing and is probably the most time consuming.  Swift has not been tested at all for production use.  It needs to be stressed significantly prior to deployment


### Reference
http://docs.openstack.org/developer/swift/associated_projects.html?highlight=php
http://developer.openstack.org/api-guide/quick-start/api-quick-start.html#openstack-api-quick-guide

token=AUTH_tk13ecae779fb341968c7e78d9a986e3ff
public url=http://swift.yosemitecloud.com:8080/v1/AUTH_2e3bdbd5-527f-4f62-adf4-10b4cdfbce1b
file url: http://swift.yosemitecloud.com:8080/v1/AUTH_2e3bdbd5-527f-4f62-adf4-10b4cdfbce1b/test/yc.xml


#### Install php Composer
+ Download Composer
```BASH
php -r "readfile('https://getcomposer.org/installer');" > composer-setup.php
php -r "if (hash_file('SHA384', 'composer-setup.php') === '7228c001f88bee97506740ef0888240bd8a760b046ee16db8f4095c0d8d525f2367663f22a46b48d072c816e7fe19959') { echo 'Installer verified'; } else { echo 'Installer corrupt'; unlink('composer-setup.php'); } echo PHP_EOL;"

php composer-setup.php

php -r "unlink('composer-setup.php');"

```
+ sudo apt-get install php5-curl
+ composer require rackspace/php-opencloud

### Install Python client
sudo apt-get install python-swiftclient

swift -A http://swift.yosemitecloud.com:8080/auth/v1.0 -U  AUTH_2e3bdbd5-527f-4f62-adf4-10b4cdfbce1b -K test stat -v

export ST_AUTH=http://swift.yosemitecloud.com:8080/auth/v1.0
export ST_USER=2e3bdbd5-527f-4f62-adf4-10b4cdfbce1b
export ST_KEY=test

curl -s -X POST http://swift.yosemitecloud.com:8080/auth/v1.0/tokens -H "Content-Type: application/json" -d '{"auth": {"tenantName": "test", "passwordCredentials": {"username": "2e3bdbd5-527f-4f62-adf4-10b4cdfbce1b", "password": "test"}}}' | python -m json.tool

swift --os-storage-url=http://swift.yosemitecloud.com:8080/v1/AUTH_2e3bdbd5-527f-4f62-adf4-10b4cdfbce1b/test --os-auth-token=AUTH_tka0cd4a502fab4a57978a463e44fc2ffb --os-tenant-name=test

### curl API
curl -i http://swift.yosemitecloud.com:8080/v1/AUTH_2e3bdbd5-527f-4f62-adf4-10b4cdfbce1b/test -X PUT -H "X-Auth-Token: AUTH_tka0cd4a502fab4a57978a463e44fc2ffb"

+ upload a file:
  + curl -i http://swift.yosemitecloud.com:8080/v1/AUTH_2e3bdbd5-527f-4f62-adf4-10b4cdfbce1b/test/proguard.conf -X PUT -H "X-Auth-Token: AUTH_tka0cd4a502fab4a57978a463e44fc2ffb" -T ./proguard.conf

+ list files
  + curl -v -H 'X-Auth-Token: AUTH_tka0cd4a502fab4a57978a463e44fc2ffb' http://swift.yosemitecloud.com:8080/v1/AUTH_2e3bdbd5-527f-4f62-adf4-10b4cdfbce1b/test
  //curl -v -H 'X-Auth-User: 2e3bdbd5-527f-4f62-adf4-10b4cdfbce1b' -H 'X-Auth-Key: test'  http://swift.yosemitecloud.com:8080/v1/AUTH_2e3bdbd5-527f-4f62-adf4-10b4cdfbce1b/test

+ set permission
https://swiftstack.com/docs/cookbooks/swift_usage/container_acl.html
  + Account Level ACL
    + curl -X POST -i -H "X-Auth-Token: <TOKEN>" \
    -H 'X-Account-Access-Control: {"admin":["AUTH_alice"],"read-only":["AUTH_readers"]}' \
    <STORAGE_URL>
  + Container Level ACL
    + curl -X <PUT|POST> -i -H "X-Auth-Token: <TOKEN>" -H "X-Container-Read: <ACL>" <STORAGE_URL>/<container>
    + curl -X PUT -i -H "X-Auth-Token: AUTH_tka0cd4a502fab4a57978a463e44fc2ffb" -H "X-Container-Read: .r:*" http://swift.yosemitecloud.com:8080/v1/AUTH_2e3bdbd5-527f-4f62-adf4-10b4cdfbce1b/test
