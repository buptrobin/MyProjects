

- ValueError: too many values to unpack

### 更新源并安装定制版Docker
#### 添加自定义源
- vi /etc/apt/sources.list
- Add below sources
```
deb http://apt.yosemitecloud.com/ trusty main
deb-src http://apt.yosemitecloud.com/ trusty main
deb http://cran.mirrors.hoobly.com/bin/linux/ubuntu trusty/
```
#### 创建facter的facts文件
- install facter
> sudo apt-get install facter
- 创建/home/robinwang/.facter/facts.d/puppetdir.yaml
```
puppetdir: '/home/robinwang/yc/devops/deploy/puppet'
devopsdir: '/home/robinwang/yc/devops/deploy'
network: 'sj'
```
- 启动docker service失败
```
sudo docker -d -D
DEBU[0000] Failed to Initialize Datastore due to datastore initialization requires a valid configuration. Operating in non-clustered mode
FATA[0000] Error starting daemon: Error initializing network controller: Error creating default "bridge" network: can't find an address range for interface "docker0"
```
